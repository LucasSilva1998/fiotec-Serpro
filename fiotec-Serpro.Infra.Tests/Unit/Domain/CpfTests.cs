using fiotec_Serpro.Domain.Exceptions;
using fiotec_Serpro.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiotec_Serpro.Infra.Tests.Unit.Domain
{
    public class CpfTests
    {
        [Theory]
        [InlineData("123.456.789-09")]
        [InlineData("12345678909")]
        public void Cpf_DeveCriarObjeto_QuandoCpfValido(string numero)
        {
            var cpf = new Cpf(numero);

            cpf.Numero.Should().Be("123.456.789-09");
        }

        [Theory]
        [InlineData("111.111.111-11")]
        [InlineData("123")]
        [InlineData("")]
        public void Cpf_DeveLancarException_QuandoCpfInvalido(string numero)
        {
            Action act = () => new Cpf(numero);

            act.Should().Throw<CpfInvalidoException>();
        }
    }
}