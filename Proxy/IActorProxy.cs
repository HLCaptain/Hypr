using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Proxy {
	public interface IActorProxy {
		Task ChangeReference(string oldPath, string newPath);
		Task<Actor> GetActor();
	}
}
