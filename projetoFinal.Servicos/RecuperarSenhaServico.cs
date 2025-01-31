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
            using (var smtpClient = new SmtpClient("smtp-relay.brevo.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("83aae9001@smtp-brevo.com", "rhWXYg7T6PLaytsc");
                smtpClient.EnableSsl = true;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("pamellaranne@gmail.com", "ProjetoFinalPamella"),
                    Subject = "Recuperação de Senha",
                    Body = string.Format(@"
            <html>
                <head>
                    <style>
                        .email-container {{
                            border: 2px solid #000000; /* borda preta em volta da div */
                            padding: 20px;
                            font-family: Arial, sans-serif;
                            width: 100%;
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #f9f9f9;
                           border-radius: 10px;
                           box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
                        }}
                        .email-header {{
                            text-align: center;
                            font-size: 24px;
                            color: #333;
                        }}
                        .email-body {{
                            font-size: 16px;
                            color: #555;
                            margin-top: 20px;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            font-size: 16px;
                            background-color:rgb(255, 255, 255);  /* Fundo azul */
                            text-decoration: none;
                            border-radius: 5px;
                            border: 2px solid #000000; 
                            box-shadow: 0px 6px 8px rgba(0, 0, 0, 0.4); /* sombra */
                            margin-top: 20px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-header'>
                            Recuperação de Senha
                        </div>
                        <div class='email-body'>
                            <p>Olá,</p>
                            <p>Recebemos um pedido de recuperação de senha. Para redefinir sua senha, clique no botão abaixo:</p>
                            <a href='http://localhost:3000/redefinir-senha?token={0}' class='button'>Redefinir Senha</a>
                            <p>Se você não fez esse pedido, ignore este email.</p>
                            <p>Atenciosamente, <br>Equipe do ProjetoFinalPamella</p>
                        </div>
                    </div>
                </body>
            </html>", token),  // Aqui, o token é inserido corretamente no link
                    IsBodyHtml = true
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

