namespace API_Nalog
{
    public class NalogResponse
    {
        public bool status { get; set; }
        public string message { get; set; }

        public NalogResponse(bool status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public override string ToString()
        {
            return $"status:  \t{status}\nmessage:\t{message}";
        }
    }
}
