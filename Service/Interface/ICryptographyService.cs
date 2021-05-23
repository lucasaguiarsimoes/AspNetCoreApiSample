using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Service.Interface
{
    public interface ICryptographyService
    {
        /// <summary>
        /// Recebe um texto e o retorna com um algoritmo de criptografia aplicado
        /// </summary>
        string Encrypt(string value);
    }
}
