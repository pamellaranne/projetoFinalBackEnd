using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace projetoFinal.Servicos.Interfaces
{
    public interface IRecuperarSenhaServico
    {
        Task EnviarEmailRecuperacaoAsync(string email, string token);
    }
}
