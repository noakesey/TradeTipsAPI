using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Domain
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TradeTipsContext>
    {
        public const string DEFAULT_DATABASE_CONNECTIONSTRING = "Server=WS2019\\SQLExpress;database=TradeTipsAPI;trusted_connection=true;";

        public TradeTipsContext CreateDbContext(string[] args)
        {
           var builder = new DbContextOptionsBuilder<TradeTipsContext>();
           builder.UseSqlServer(DEFAULT_DATABASE_CONNECTIONSTRING,
               opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
           );

           return new TradeTipsContext(builder.Options);
        }
    }
}
