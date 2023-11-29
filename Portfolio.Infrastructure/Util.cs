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

        public static int GeraDigitoModulo11(string numero)
        {
            int[] intPesos = { 2, 3, 4, 5, 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9 };

            var intSoma = 0;
            var intIdx = 0;

            for (var intPos = numero.Length - 1; intPos >= 0; intPos--)
            {
                intSoma += Convert.ToInt32(numero[intPos].ToString()) * intPesos[intIdx];
                if (intIdx == 9)
                {
                    intIdx = 2;
                }
                else
                {
                    intIdx++;
                }
            }

            var intResto = intSoma * 10 % 11;
            var intDigito = intResto;
            if (intDigito >= 10)
                intDigito = 0;

            return intDigito;
        }

        public static int GeraDigitoModulo10(string numero)
        {
            numero = DeixaNumeros(numero);
            numero = numero.PadLeft(12, '0');
            // Baseado no calculo fornecido pelo Edilson
            int[] intPesos = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };

            var intSoma = 0;

            for (var intPos = 0; intPos < numero.Length; intPos++)
            {
                var produto = Convert.ToInt32(numero[intPos].ToString()) * intPesos[intPos];

                if (produto > 9)
                {
                    produto = Convert.ToInt32(produto.ToString().Substring(0, 1)) + Convert.ToInt32(produto.ToString().Substring(1, 1));
                }

                intSoma += produto;
            }

            var intResto = intSoma % 10;

            var intDigito = 10 - intResto;

            if (intDigito > 9)
                intDigito = 0;

            return intDigito;
        }

        public static string MascaraCartao(string cartao)
        {
            if (cartao.Length == 16)
            {
                return cartao.Substring(0, 4) + "." +
                       cartao.Substring(4, 4) + "." +
                       cartao.Substring(8, 4) + "." +
                       cartao.Substring(12, 4);
            }
            return cartao;
        }

        public static string MascaraCep(string cep)
        {
            return cep.Substring(0, 2) + cep.Substring(2, 3) + "-" + cep.Substring(5, 3);
        }

        public static string MascaraCelular(string celular)
        {
            return $"({celular.Substring(0, 2)}) {celular.Substring(2, 5)}-{celular.Substring(5, 4)}";
        }

        public static string MascaraDocFederal(string doc)
        {
            if (doc.Length == 11)
            {
                return doc.Substring(0, 3) + "." + doc.Substring(3, 3) + "." + doc.Substring(6, 3) + "-" + doc.Substring(9, 2);
            }
            if (doc.Length == 14)
            {
                return doc.Substring(0, 2) + "." + doc.Substring(2, 3) + "." + doc.Substring(5, 3) + "/" + doc.Substring(8, 4) + "-" + doc.Substring(12, 2);
            }
            return doc;
        }

        public static string CreatePassword()
        {
            var tamanho = 8;

            var valormaximo = SenhaCaracteresValidos.Length - 1;

            var random = new Random(DateTime.Now.Millisecond);

            var senhaBuilder = new StringBuilder(tamanho);

            for (var i = 0; i < tamanho; i++)
                senhaBuilder.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);

            var senha = senhaBuilder.ToString();

            if (!senha.Any(char.IsDigit))
            {
                var posNum = random.Next(0, Numeros.Length - 1);
                var posSenha = random.Next(0, senha.Length - 1);
                senhaBuilder[posSenha] = Numeros[posNum];
            }
            if (!senha.Any(c => char.IsLetter(c) && char.IsUpper(c)))
            {
                var posCaracter = random.Next(0, LetrasMaiusculas.Length - 1);
                var posSenha = random.Next(0, senha.Length - 1);
                senhaBuilder[posSenha] = LetrasMaiusculas[posCaracter];
            }
            if (!senha.Any(c => char.IsLetter(c) && char.IsLower(c)))
            {
                var posCaracter = random.Next(0, LetrasMinusculas.Length - 1);
                var posSenha = random.Next(0, senha.Length - 1);
                senhaBuilder[posSenha] = LetrasMinusculas[posCaracter];
            }

            return senhaBuilder.ToString();
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

            return numcel.StartsWith("6") ||
                   numcel.StartsWith("7") ||
                   numcel.StartsWith("8") ||
                   numcel.StartsWith("9");
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

        public static string RemoveZeroEsquerda(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return long.Parse(input).ToString();
        }
    }
}