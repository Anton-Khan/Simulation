using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_2
{
    public class Data
    {
        public double Healthy { get; set; }
        public double Latent { get; set; }
        public double Ill { get; set; }
        public double Recovered { get; set; }
        public Data Copy() { return new Data() { Healthy = Math.Ceiling(this.Healthy), Ill = Math.Ceiling(this.Ill), Latent = Math.Ceiling(this.Latent), Recovered = Math.Ceiling(this.Recovered) }; }
    }
}
