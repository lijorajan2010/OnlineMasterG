using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace OnlineMasterG.Base
{
	public class ServiceBase
	{
		#region Consts
		private const string DBContextName = "DBContext";
		#endregion

		#region Properties
		public static OnlinemasterjiEntities DB
		{
			get
			{
				// Find DB on this thread's bag
				OnlinemasterjiEntities db = CallContext.GetData(DBContextName) as OnlinemasterjiEntities;

				// If it doesn't exists, create it
				if (db == null)
				{
					db = new OnlinemasterjiEntities();

					// Save the context on the thread's bag
					CallContext.SetData(DBContextName, db);
				}

				return db;
			}
		}
		#endregion

		#region Methods
		public static void ResetContext()
		{
			// Remove and dispose current context
			DisposeContext();

			// Create new context
			CallContext.SetData(DBContextName, new OnlinemasterjiEntities());
		}

		public static void DisposeContext()
		{
			// Read current context
			OnlinemasterjiEntities db = CallContext.GetData(DBContextName) as OnlinemasterjiEntities;

			// If there's a context
			if (db != null)
			{
				// Dispose it.. freeing any connection and resources used
				db.Dispose();

				// Remove context instance from the thread's bag
				CallContext.SetData(DBContextName, null);
			}
		}
		#endregion
	}
}