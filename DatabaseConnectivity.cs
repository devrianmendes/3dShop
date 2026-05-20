using System;

public abstract class DatabaseConnectivity
{
    public void Execute()
    {
        //using var teste = new AppDbContext(options);
        try
        {
            using var dbInstance = new AppDbContext(options);
            bool conTest = dbInstance.Database.CanConnect();

            if (conTest)
            {
                Console.WriteLine("Connection success.");

            }
            else
            {
                Console.WriteLine("Connection failure.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao conectar: " + ex.Message);
        }

    }
}
