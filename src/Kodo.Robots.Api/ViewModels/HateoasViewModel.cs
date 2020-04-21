using System;
using System.Collections.Generic;

namespace Kodo.Robots.Api.ViewModels
{
    public class HateoasViewModel
    {
        public HateoasViewModel()
        {
            Links = new List<HateoasLinkViewModel>();
        }

        public List<HateoasLinkViewModel> Links { get; set; }
    }
}
