using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ERP.Backup;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== ChurrasTech ERP - Sistema de Backup ===");
        Console.WriteLine();

        if (args.Length == 0)
        {
            ShowUsage();
            return;
        }

        try
        {
            switch (args[0].ToLower())
            {
                case "backup":
                    var database = args.Length > 1 ? args[1] : "churrastech_prod";
                    await CreateBackup(database);
                    break;
                case "restore":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Uso: ERP.Backup restore <database> <arquivo_backup>");
                        return;
                    }
                    await RestoreBackup(args[1], args[2]);
                    break;
                case "schedule":
                    await ScheduleAutomaticBackup();
                    break;
                default:
                    ShowUsage();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            Environment.Exit(1);
        }
    }

    static void ShowUsage()
    {
        Console.WriteLine("Uso:");
        Console.WriteLine("  ERP.Backup backup [database]        - Criar backup do banco");
        Console.WriteLine("  ERP.Backup restore <db> <arquivo>   - Restaurar backup");
        Console.WriteLine("  ERP.Backup schedule                 - Configurar backup automático");
        Console.WriteLine();
        Console.WriteLine("Exemplo:");
        Console.WriteLine("  ERP.Backup backup churrastech_prod");
        Console.WriteLine("  ERP.Backup restore churrastech_prod backup_20231226.sql");
    }

    static async Task CreateBackup(string database)
    {
        Console.WriteLine($"Criando backup do banco: {database}");

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var backupFile = $"backup_{database}_{timestamp}.sql";
        var backupPath = Path.Combine(Directory.GetCurrentDirectory(), "backups");

        // Create backups directory if it doesn't exist
        Directory.CreateDirectory(backupPath);
        var fullBackupPath = Path.Combine(backupPath, backupFile);

        var connectionString = GetConnectionString(database);

        try
        {
            // Use mysqldump for backup
            var arguments = $"--user=root --password= --host=localhost --single-transaction --routines --triggers {database}";
            
            var processInfo = new ProcessStartInfo
            {
                FileName = "mysqldump",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process != null)
            {
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    await File.WriteAllTextAsync(fullBackupPath, output);
                    Console.WriteLine($"✓ Backup criado com sucesso: {fullBackupPath}");
                    Console.WriteLine($"  Tamanho: {new FileInfo(fullBackupPath).Length / 1024} KB");
                }
                else
                {
                    Console.WriteLine($"Erro no mysqldump: {error}");
                    Console.WriteLine("Tentando backup alternativo...");
                    await CreateAlternativeBackup(database, fullBackupPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao executar mysqldump: {ex.Message}");
            Console.WriteLine("Tentando backup alternativo...");
            await CreateAlternativeBackup(database, fullBackupPath);
        }
    }

    static async Task CreateAlternativeBackup(string database, string backupPath)
    {
        var connectionString = GetConnectionString(database);

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        var backupContent = new List<string>();
        backupContent.Add($"-- ChurrasTech ERP Backup");
        backupContent.Add($"-- Database: {database}");
        backupContent.Add($"-- Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        backupContent.Add($"-- Generator: ERP.Backup");
        backupContent.Add("");

        // Get all tables
        var tablesQuery = "SHOW TABLES";
        using var tablesCommand = new MySqlCommand(tablesQuery, connection);
        using var tablesReader = await tablesCommand.ExecuteReaderAsync();

        var tables = new List<string>();
        while (await tablesReader.ReadAsync())
        {
            tables.Add(tablesReader.GetString(0));
        }
        tablesReader.Close();

        foreach (var table in tables)
        {
            backupContent.Add($"-- Table: {table}");
            
            // Get table structure
            var createTableQuery = $"SHOW CREATE TABLE `{table}`";
            using var createCommand = new MySqlCommand(createTableQuery, connection);
            using var createReader = await createCommand.ExecuteReaderAsync();
            
            if (await createReader.ReadAsync())
            {
                backupContent.Add($"DROP TABLE IF EXISTS `{table}`;");
                backupContent.Add(createReader.GetString(1) + ";");
                backupContent.Add("");
            }
            createReader.Close();

            // Get table data
            var dataQuery = $"SELECT * FROM `{table}`";
            using var dataCommand = new MySqlCommand(dataQuery, connection);
            using var dataReader = await dataCommand.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                backupContent.Add($"LOCK TABLES `{table}` WRITE;");
                var insertStatement = $"INSERT INTO `{table}` VALUES ";
                var values = new List<string>();

                while (await dataReader.ReadAsync())
                {
                    var row = new List<string>();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var value = dataReader.IsDBNull(i) ? "NULL" : $"'{dataReader.GetValue(i).ToString()?.Replace("'", "''")}'";
                        row.Add(value);
                    }
                    values.Add($"({string.Join(",", row)})");
                }

                if (values.Count > 0)
                {
                    backupContent.Add(insertStatement + string.Join(",", values) + ";");
                }
                backupContent.Add("UNLOCK TABLES;");
                backupContent.Add("");
            }
            dataReader.Close();
        }

        await File.WriteAllLinesAsync(backupPath, backupContent);
        Console.WriteLine($"✓ Backup alternativo criado com sucesso: {backupPath}");
        Console.WriteLine($"  Tamanho: {new FileInfo(backupPath).Length / 1024} KB");
    }

    static async Task RestoreBackup(string database, string backupFile)
    {
        Console.WriteLine($"Restaurando backup: {backupFile} para {database}");

        if (!File.Exists(backupFile))
        {
            Console.WriteLine($"Arquivo de backup não encontrado: {backupFile}");
            return;
        }

        var connectionString = GetConnectionString(database);

        try
        {
            // Use mysql command line for restore
            var arguments = $"--user=root --password= --host=localhost {database}";
            
            var processInfo = new ProcessStartInfo
            {
                FileName = "mysql",
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process != null)
            {
                var backupContent = await File.ReadAllTextAsync(backupFile);
                await process.StandardInput.WriteAsync(backupContent);
                process.StandardInput.Close();

                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("✓ Backup restaurado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Erro no mysql: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao restaurar backup: {ex.Message}");
        }
    }

    static async Task ScheduleAutomaticBackup()
    {
        Console.WriteLine("=== Configuração de Backup Automático ===");
        Console.WriteLine();
        Console.WriteLine("Esta funcionalidade criaria um agendamento para backup automático.");
        Console.WriteLine("Em um sistema real, seria implementada usando:");
        Console.WriteLine("- Windows Task Scheduler (Windows)");
        Console.WriteLine("- cron jobs (Linux)");
        Console.WriteLine("- Serviços do Windows");
        Console.WriteLine();
        Console.WriteLine("Exemplo de comando cron para backup diário às 2h:");
        Console.WriteLine("0 2 * * * /path/to/ERP.Backup backup churrastech_prod");
        Console.WriteLine();
        
        await Task.CompletedTask;
    }

    static string GetConnectionString(string database)
    {
        return $"server=localhost;database={database};uid=root;pwd=;";
    }
}
