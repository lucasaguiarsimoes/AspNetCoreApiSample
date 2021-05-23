using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Service.Interface;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Service.Services
{
    public class CryptographySHA256Service : ICryptographyService
    {
        /// <summary>
        /// Recebe um texto e o retorna com um algoritmo de criptografia aplicado
        /// </summary>
        public string Encrypt(string value)
        {
            // Define uma chave privada fixa para criptografar a senha (Para fortalecer a segurança, cada usuário pode ter seu próprio hash auto-gerado também)
            string hash = "66B67248-9225-4325-B425-99A4EFEFE129";

            // Concatena a senha com mais algumas informações
            string passwordPlus = value + hash + value.Length;

            // Criptografa o texto definido
            string passwordSHA256 = CryptographySHA256(passwordPlus);

            // Segunda iteração para aumentar a complexidade do hash da senha
            passwordSHA256 = CryptographySHA256(passwordSHA256 + hash + passwordSHA256.Length);

            // Retorna a senha calculadas
            return passwordSHA256;
        }

        /// <summary>
        /// Aplica a criptografia SHA256 utilizando uma string
        /// </summary>
        private string CryptographySHA256(string texto)
        {
            StringBuilder builder = new StringBuilder();

            // Instancia o objeto para criptografar o valor recebido
            using (SHA256 objSHA = SHA256.Create())
            {
                // Utiliza o encoding UTF8 para obter byte a byte do valor recebido
                Encoding objEncoding = Encoding.UTF8;
                byte[] hash = objSHA.ComputeHash(objEncoding.GetBytes(texto));

                // Converte cada byte obtido para hexadecimal (ToString("x2") = 2 caracteres Hexadecimais uppercase para cada byte)
                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
            }

            return builder.ToString();
        }
    }
}
