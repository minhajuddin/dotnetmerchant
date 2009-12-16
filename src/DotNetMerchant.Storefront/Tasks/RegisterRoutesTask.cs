﻿using System.Web.Mvc;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;

namespace DotNetMerchant.Storefront.Tasks
{
    public class RegisterRoutesTask : IBootstrapperTask
    {
        private readonly RouteCollection _routes;

        public RegisterRoutesTask()
            : this(RouteTable.Routes)
        {

        }

        public RegisterRoutesTask(RouteCollection routes)
        {
            _routes = routes;
        }

        public void Execute()
        {
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            AreaRegistration.RegisterAllAreas();
            
            HasExecuted = true;
        }

        public bool HasExecuted { get; private set; }
    }
}
