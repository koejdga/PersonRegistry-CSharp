using Person.Exceptions;

namespace Person.Models
{
    public class EmailAddressModel
    {

        private string _emailAddress;


        public EmailAddressModel(string str) 
        {
            ValidateInput(str);
            _emailAddress = str;
        }

        public void Change(string new_address)
        {
            if (new_address != _emailAddress)
            {
                ValidateInput(new_address);
                _emailAddress = new_address;
            }
        }

        public override string ToString()
        {
            return _emailAddress;
        }

        private void ValidateInput(string str)
        {
            string[] parts = str.Split('@');

            if (parts.Length != 2)
            {
                throw new InvalidEmailAddressException("Email address should consist of local part" +
                    " and domain part separated by one '@' symbol");
            }

            if (string.IsNullOrEmpty(parts[0]))
            {
                throw new InvalidEmailAddressException("Local part cannot be empty");
            }

            if (string.IsNullOrEmpty(parts[1]))
            {
                throw new InvalidEmailAddressException("Domain part cannot be empty");
            }

        }
    }
}
