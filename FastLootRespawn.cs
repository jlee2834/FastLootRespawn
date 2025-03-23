using System.Collections.Generic;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("FastLootRespawn", "jlee2834", "1.0.1")]
    [Description("Halves the respawn time of loot crates at monuments.")]

    public class FastLootRespawn : RustPlugin
    {
        List<string> lootKeywords = new List<string>
        {
            "crate", "elite", "blue", "red", "green", "card", "tech", "medical"
        };

        void OnServerInitialized()
        {
            var allGroups = GameObject.FindObjectsOfType<SpawnGroup>();

            int modified = 0;
            foreach (var group in allGroups)
            {
                if (group == null || group.spawnPoints == null || group.spawnPoints.Length == 0)
                    continue;

                string name = group.name?.ToLower() ?? "";
                bool isLootGroup = false;
                foreach (var keyword in lootKeywords)
                {
                    if (name.Contains(keyword))
                    {
                        isLootGroup = true;
                        break;
                    }
                }

                if (!isLootGroup)
                    continue;

                if (group.respawnDelayMin != float.PositiveInfinity)
                    group.respawnDelayMin = Mathf.Max(group.respawnDelayMin / 2f, 1f);

                if (group.respawnDelayMax != float.PositiveInfinity)
                    group.respawnDelayMax = Mathf.Max(group.respawnDelayMax / 2f, 1f);

                modified++;
            }

            Puts($"FastLootRespawn: Modified {modified} loot crate spawn groups.");
        }
    }
}
