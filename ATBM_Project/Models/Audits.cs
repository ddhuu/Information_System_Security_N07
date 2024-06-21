namespace ATBM_Project.Models
{
    public class Audits
    {
        public string SESSION_ID { get; set; }
        public string USERNAME { get; set; }
        public string ACTION_NAME { get; set; }
        public string OBJECT_NAME { get; set; }
        public string TIMESTAMP { get; set; }

        public string STATUS { get; set; }

        public string SQL_TEXT { get; set; }

    }
}
