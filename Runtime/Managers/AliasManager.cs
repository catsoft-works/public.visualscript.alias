using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualScript.Runtime.Alias
{
    [AddComponentMenu("")]
    public class AliasManager : Singleton<AliasManager>
    {
        private const string ERR_DUPLICATE_ID = "Alias with duplicate ID = {0}";
        private const string ERR_CHANGE_ID = "Alias change with duplicate ID from {0} to {1}";

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dictionary<IdString, GameObject> m_Aliases;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            this.m_Aliases = new Dictionary<IdString, GameObject>();
        }

        // REGISTER METHODS: ----------------------------------------------------------------------

        internal static void Register(Alias alias)
        {
            if (AppManager.IsExiting) return;
            if (alias == null) return;

            IdString id = alias.Id;
            GameObject instance = alias.gameObject;

            if (!Instance.m_Aliases.TryAdd(id, instance))
            {
                Debug.LogErrorFormat(ERR_DUPLICATE_ID, id);
            }

            alias.EventChange += OnChangeID;
        }

        internal static void Unregister(Alias alias)
        {
            if (AppManager.IsExiting) return;
            if (alias == null) return;

            alias.EventChange -= OnChangeID;
            Instance.m_Aliases.Remove(alias.Id);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static bool Has(IdString id)
        {
            return Instance.m_Aliases.ContainsKey(id);
        }

        public static GameObject Get(IdString id)
        {
            return Instance.m_Aliases.GetValueOrDefault(id);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void OnChangeID(IdString previousID, IdString newID)
        {
            GameObject instance = Get(previousID);
            Instance.m_Aliases.Remove(previousID);

            if (!Instance.m_Aliases.TryAdd(newID, instance))
            {
                Debug.LogErrorFormat(ERR_CHANGE_ID, previousID, newID);
            }
        }
    }
}