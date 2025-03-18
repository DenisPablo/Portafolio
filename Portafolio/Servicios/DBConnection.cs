namespace Portafolio.Servicios
{
    public interface IDbConnection
    {
        public string GetConnectionString();
    }

    class Dbconnection : IDbConnection
    {
        private readonly string DB_SERVER;
        private readonly string DB_NAME;
        private readonly string DB_USER;
        private readonly string DB_PASSWORD;

        public Dbconnection(IConfiguration configuration)
        {
            DB_SERVER = configuration["DB_SERVER"];
            DB_NAME = configuration["DB_NAME"];
            DB_USER = configuration["DB_USER"];
            DB_PASSWORD = configuration["DB_PASSWORD"];
        }

        public string GetConnectionString()
        {

            var connectionString = "Server=" + DB_SERVER + ";Database=" + DB_NAME + ";User Id=" + DB_USER + ";Password=" + DB_PASSWORD + ";TrustServerCertificate=True;Encrypt=False;";
            return connectionString;
        }
    }
}

