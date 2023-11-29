using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Portfolio.Infrastructure
{
    public static class Util
    {
        private const string SenhaCaracteresValidos = LetrasMaiusculas + LetrasMinusculas + Numeros;
        private const string LetrasMaiusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LetrasMinusculas = "abcdefghijklmnopqrstuvwxyz";
        private const string Numeros = "1234567890";

        public static string RemoveAcentos(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (var letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            return sbReturn.ToString();
        }

        public static string DeixaNumeros(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? string.Empty : string.Join("", Regex.Split(texto, @"[^\d]", RegexOptions.None, new TimeSpan(0, 0, 30)));
        }

        public static bool ValidaDocumento(string documento)
        {
            documento = DeixaNumeros(documento);

            return documento.Length == 11 ? ValidaCpf(documento) : ValidaCnpj(documento);
        }

        public static bool ValidaCnpj(string cnpj)
        {
            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto;
            return cnpj.EndsWith(digito);
        }

        public static bool ValidaCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = DeixaNumeros(cpf);

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (var i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            var digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto;
            return cpf.EndsWith(digito);
        }

        public static bool ValidaEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidaSexo(string sexo)
        {
            if (sexo.ToUpper() == "M" || sexo.ToUpper() == "F")
                return true;

            return false;
        }

        public static bool SenhaValida(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("A senha não pode ser nula ou vazia.");

            if (senha.Length < 6)
                throw new ArgumentException("A senha deve conter pelo menos 6 caracteres.");

            if (senha.Contains(" "))
                throw new ArgumentException("A senha não pode conter espaços.");

            bool temLetra = false;
            bool temNumero = false;
            bool temUpper = false;
            bool temLower = false;

            foreach (var c in senha)
            {
                if (char.IsLetter(c))
                {
                    temLetra = true;
                    if (char.IsUpper(c)) temUpper = true;
                    if (char.IsLower(c)) temLower = true;
                }
                else if (char.IsNumber(c))
                {
                    temNumero = true;
                }
            }

            if (temNumero && temLetra && temUpper && temLower)
                return true;

            throw new ArgumentException("A senha deve conter letras maiúsculas, minúsculas, e números.");
        }


        public static bool ValidaNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            var nomes = nome.Split(' ');
            return nomes.Length > 1 && nomes[0].Length >= 2;
        }

        public static bool ValidaTelefone(string telefone)
        {
            if (!ValidaApenasNumeros(telefone))
                return false;

            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            if (telefone.Length < 10)
                return false;

            var cel = telefone;
            var numcel = cel.Substring(2);

            return numcel.Length >= 8;
        }

        public static bool ValidaCelular(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
            {
                return false;
            }

            if (telefone.Length < 11)
            {
                return false;
            }

            var cel = DeixaNumeros(telefone);
            var numcel = cel.Substring(2);
            if (numcel.Length != 9)
            {
                return false;
            }

            return numcel.StartsWith('6') ||
                   numcel.StartsWith('7') ||
                   numcel.StartsWith('8') ||
                   numcel.StartsWith('9');
        }

        public static bool ValidaApenasNumeros(string str)
        {
            return str.All(c => c >= '0' && c <= '9');
        }

        public static bool ValidaApenasLetras(string str)
        {
            var temp = str.ToUpper();
            return temp.All(c => c >= 'A' && c <= 'Z');
        }
    }
}