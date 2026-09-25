using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualScript.Runtime.Alias
{
    [AddComponentMenu("")]
    public class AliasManager : Singleton<AliasManager>
    {
        private const string ERR_DUPLICATE_ID = "Duplicate Alias ID = {0}. '{1}' not set because '{2}' uses it";

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

            AliasManager instance = Instance;
            if (instance == null) return;

            IdString id = alias.Id;
            GameObject gameObject = alias.gameObject;

            if (instance.m_Aliases.TryGetValue(id, out GameObject registered))
            {
                if (registered == gameObject) return;

                if (registered != null)
                {
                    Debug.LogErrorFormat(
                        gameObject,
                        ERR_DUPLICATE_ID,
                        id,
                        gameObject.name,
                        registered.name
                    );

                    return;
                }
            }

            instance.m_Aliases[id] = gameObject;
        }

        internal static void Unregister(Alias alias)
        {
            if (AppManager.IsExiting) return;
            if (alias == null) return;

            Remove(alias.Id, alias.gameObject);
        }

        internal static void ChangeId(Alias alias, IdString previousId)
        {
            if (AppManager.IsExiting) return;
            if (alias == null) return;

            Remove(previousId, alias.gameObject);
            Register(alias);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static bool Has(IdString id)
        {
            return Get(id) != null;
        }

        public static GameObject Get(IdString id)
        {
            AliasManager instance = Instance;
            if (instance == null) return null;

            if (!instance.m_Aliases.TryGetValue(id, out GameObject alias)) return null;
            if (alias != null) return alias;

            instance.m_Aliases.Remove(id);
            return null;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static void Remove(IdString id, GameObject gameObject)
        {
            AliasManager instance = Instance;
            if (instance == null) return;

            if (!instance.m_Aliases.TryGetValue(id, out GameObject registered)) return;
            if (registered != null && registered != gameObject) return;

            instance.m_Aliases.Remove(id);
        }
    }
}