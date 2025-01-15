using System;
using System.Net;
using System.Net.Mail;
using projetoFinal.Servicos.Interfaces;


public class RecuperarSenhaServico : IRecuperarSenhaServico
{
    public async Task EnviarEmailRecuperacaoAsync(string email, string token)
    {
        try
        {
            using (var smtpClient = new SmtpClient("smtp-relay.brevo.com",
            587))
            {
                smtpClient.Credentials = new
                NetworkCredential("838f4c001@smtp-brevo.com", "Cvxa1VIHNdwK9B0n"); 
                smtpClient.EnableSsl = true;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("walterfonseca1606@gmail.com", "ProjetoFinalPamella"),
                    Subject = "Recuperação de Senha",
                    Body = $"Aqui está seu token de recuperação: {token}",
                    IsBodyHtml = false
                };
                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine($"Erro SMTP: {smtpEx.Message} - {smtpEx.StatusCode}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro geral ao enviar e-mail: {ex.Message}");
            throw;
        }
    }
}

