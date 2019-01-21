﻿using events.tac.local.Models;
using System.Collections.Generic;
using System.Linq;
using TAC.Sitecore.Abstractions.Interfaces;
using TAC.Sitecore.Abstractions.SitecoreImplementation;

namespace events.tac.local.Business
{
    public class BreadcrumbBuilder
    {
        private IRenderingContext _context;

        public BreadcrumbBuilder() : this(SitecoreRenderingContext.Create()) { }

        public BreadcrumbBuilder(IRenderingContext context)
        {
            this._context = context;
        }

        public IEnumerable<NavigationItem> Build()
        {
            return _context?.HomeItem == null || _context?.ContextItem == null ?
                Enumerable.Empty<NavigationItem>() :
                _context
                    .ContextItem
                    .GetAncestors()
                    .Where(i => _context.HomeItem.IsAncestorOrSelf(i))
                    .Select(i => new NavigationItem
                        (
                            title: i.DisplayName,
                            url: i.Url,
                            active: false
                        ))
                    .Concat(
                    new[]
                    {
                        new NavigationItem
                         (
                            title : _context.ContextItem.DisplayName,
                            url : _context.ContextItem.Url,
                            active : true
                        )
                    }
                    );

        }
    }
}