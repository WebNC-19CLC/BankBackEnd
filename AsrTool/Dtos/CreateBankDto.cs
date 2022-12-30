namespace AsrTool.Dtos
{
    public class CreateBankDto
    {
        public string Name { get; set; }
        public string API { get; set; }

        public string EncryptRsaPublicKey { get; set; }

        public string DecryptRsaPrivateKey { get; set; }

        public string DecryptPublicKey { get; set; }
    }
}
