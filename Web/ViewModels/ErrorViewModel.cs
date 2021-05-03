using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.ViewModels
{
    public class ErrorViewModel
    {
        /// <summary>
        /// Http Status code definido para a falha em questão
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// Mensagem da falha definida pelo server
        /// </summary>
        public string error { get; set; }

        public ErrorViewModel(int statusCode, string serverMessage)
        {
            this.status = statusCode;
            this.error = serverMessage;
        }
    }
}
