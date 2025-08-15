using System;
using System.Linq;
using System.Text.RegularExpressions;
using fiotec_Serpro.Domain.Exceptions;

namespace fiotec_Serpro.Domain.ValueObjects
{
    public class Cpf
    {
        public string Numero { get; private set; }

        protected Cpf() { }

        public Cpf(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new CpfInvalidoException("CPF não pode ser vazio.");

            numero = ApenasDigitos(numero);

            if (numero.Length != 11 || !CpfValido(numero))
                throw new CpfInvalidoException("CPF inválido.");

            Numero = Formatar(numero);
        }

        private string ApenasDigitos(string valor)
        {
            return Regex.Replace(valor, "[^0-9]", "");
        }

        private string Formatar(string cpf)
        {
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        private bool CpfValido(string cpf)
        {
            if (cpf.Distinct().Count() == 1) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            tempCpf += resto;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(resto.ToString());
        }

        public override string ToString() => Numero;

        public override bool Equals(object obj)
        {
            if (obj is not Cpf outro) return false;
            return Numero == outro.Numero;
        }

        public override int GetHashCode() => Numero.GetHashCode();
    }
}
