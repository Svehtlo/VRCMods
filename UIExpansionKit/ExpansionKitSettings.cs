using System.Collections.Generic;
using System.Linq;
using MelonLoader;

namespace UIExpansionKit
{
    public static class ExpansionKitSettings
    {
        private const string KitCategory = "UIExpansionKit";
        private const string PinnedPrefs = "PinnedPrefs";
        private const string QmExpandoStartsCollapsed = "QmExpandoStartsCollapsed";
        private const string CategoriesStartCollapsed = "CategoriesStartCollapsed";
        private const string QmExpandoMinRows = "QmExpandoMinRows";
        private const string QmExpandoMaxRows = "QmExpandoMaxRows";

        internal static void RegisterSettings()
        {
            MelonPrefs.RegisterCategory(KitCategory,"UI Expansion Kit");
            
            MelonPrefs.RegisterString(KitCategory, PinnedPrefs, "", hideFromList: true);
            
            MelonPrefs.RegisterBool(KitCategory, QmExpandoStartsCollapsed, false, "Quick Menu extra panel starts hidden");
            MelonPrefs.RegisterBool(KitCategory, CategoriesStartCollapsed, false, "Settings categories start collapsed");
            
            MelonPrefs.RegisterInt(KitCategory, QmExpandoMinRows, 1, "Minimum rows in Quick Menu extra panel");
            MelonPrefs.RegisterInt(KitCategory, QmExpandoMaxRows, 3, "Maximum rows in Quick Menu extra panel");
        }

        public static bool IsQmExpandoStartsCollapsed() => MelonPrefs.GetBool(KitCategory, QmExpandoStartsCollapsed);
        public static bool IsCategoriesStartCollapsed() => MelonPrefs.GetBool(KitCategory, CategoriesStartCollapsed);

        public static int ClampQuickMenuExpandoRowCount(int targetCount)
        {
            var min = MelonPrefs.GetInt(KitCategory, QmExpandoMinRows);
            var max = MelonPrefs.GetInt(KitCategory, QmExpandoMaxRows);

            if (targetCount < min) return min;
            if (targetCount > max) return max;
            return targetCount;
        }

        public static void PinPref(string category, string prefName)
        {
            SetPinnedPrefs(ListPinnedPrefs(true).Concat(new []{(category, prefName)}).Distinct());
        }
        
        public static void UnpinPref(string category, string prefName)
        {
            SetPinnedPrefs(ListPinnedPrefs(true).Where(it => it != (category, prefName)));
        }
        
        internal static void SetPinnedPrefs(IEnumerable<(string category, string name)> prefs)
        {
            var raw = string.Join(";", prefs.Select(it => $"{it.category},{it.name}"));
            var prefDesc = MelonPrefs.GetPreferences()[KitCategory][PinnedPrefs];
            prefDesc.ValueEdited = raw;
        }

        public static IEnumerable<(string category, string name)> ListPinnedPrefs(bool fromTempStore)
        {
            var raw = fromTempStore ? MelonPrefs.GetPreferences()[KitCategory][PinnedPrefs].ValueEdited ?? "" : MelonPrefs.GetString(KitCategory, PinnedPrefs) ?? "";
            var parts = raw.Split(';');
            return parts.Select(it => it.Split(',')).Where(it => it.Length == 2).Select(it => (it[0], it[1]));
        }
    }
}