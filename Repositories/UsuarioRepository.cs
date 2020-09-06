using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;

namespace NFCE.API.Repositories
{
    public class UsuarioRepository : RepositoryBase<UsuarioModel>, IUsuarioRepository
    {
        public UsuarioRepository(IConfiguration config) : base(config)
        {
            
        }
    }
}