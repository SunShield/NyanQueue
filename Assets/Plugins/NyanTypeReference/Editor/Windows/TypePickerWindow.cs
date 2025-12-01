using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NyanTypeReference.Attribs;
using NyanTypeReference.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace NyanTypeReference.Editor.Windows
{
    public class TypePickerWindow : EditorWindow
    {
        private static Action<Type> _onPicked;
        private static Type _currentType;
        private static TypeOptionsAttribute _options;
        private string _search = "";
        private Vector2 _scroll;
        private static List<Type> _cachedTypes;
        private List<Type> _filteredTypes;
        private bool _initialized;

        public static void Show(Rect activatorRect, Type current, Action<Type> onPicked, TypeOptionsAttribute options)
        {
            _currentType = current;
            _onPicked = onPicked;
            _options = options;

            var window = CreateInstance<TypePickerWindow>();
            window.ShowAsDropDown(activatorRect, new Vector2(600, 400));
        }

        private void OnEnable()
        {
            wantsMouseMove = true;
        }

        private void OnLostFocus() => Close();

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            BuildTypeCache();
            FilterList();
        }

        private void OnGUI()
        {
            Initialize();

            DrawToolbar();
            DrawList();

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                Close();
                GUIUtility.ExitGUI();
            }
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();

            GUILayout.Label("Search:", GUILayout.Width(55));

            var searchRect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));
            var clearRect  = new Rect(searchRect.xMax - 18, searchRect.y + 2, 16, 16);
            var inputRect  = new Rect(searchRect.x,     searchRect.y, searchRect.width - 20, searchRect.height);

            var newSearch = EditorGUI.TextField(inputRect, _search);

            if (!string.Equals(newSearch, _search, StringComparison.Ordinal))
            {
                _search = newSearch.Trim();
                FilterList();
                Repaint();
            }

            if (GUI.Button(clearRect, "×"))
            {
                _search = "";
                FilterList();

                GUI.FocusControl(null);
                Repaint();
            }

            if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(22))) Close();

            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawList()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            if (_options == null || _options.AllowNone)
            {
                if (GUILayout.Button("(None)", GetStyleFor(null)))
                    Pick(null);
            }

            foreach (var t in _filteredTypes)
            {
                if (GUILayout.Button(t.FullName, GetStyleFor(t))) Pick(t);
            }

            EditorGUILayout.EndScrollView();
        }

        private GUIStyle GetStyleFor(Type t)
        {
            var s = new GUIStyle(EditorStyles.label);
            if (t == _currentType) s.fontStyle = FontStyle.Bold;
            return s;
        }

        private void Pick(Type t)
        {
            _onPicked?.Invoke(t);
            Close();
            GUIUtility.ExitGUI();
        }

        private void BuildTypeCache()
        {
            if (_cachedTypes != null) return;

            var baseType = _options?.BaseType;
            var includeAbstract = _options?.IncludeAbstract ?? false;
            var includeInterfaces = _options?.IncludeInterfaces ?? false;

            var list = new List<Type>();

            var assemblies = AssemblyFilterUtility.GetRelevantAssemblies(baseType);

            foreach (var asm in assemblies)
            {
                Type[] types;
                try { types = asm.GetTypes(); }
                catch (ReflectionTypeLoadException e) { types = e.Types.Where(x => x != null).ToArray(); }

                foreach (var t in types)
                {
                    if (t == null) continue;
                    if (t.Name.StartsWith("<")) continue;
                    if (t.IsNestedPrivate) continue;
                    if (t.IsGenericTypeDefinition) continue;
                    if (t.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false)) continue;
                    if (baseType != null && !baseType.IsAssignableFrom(t)) continue;
                    if (t.IsInterface && !includeInterfaces) continue;
                    if (t.IsAbstract && !includeAbstract) continue;

                    list.Add(t);
                }
            }

            list.Sort((a, b) => string.Compare(a.FullName, b.FullName, StringComparison.Ordinal));
            _cachedTypes = list;
        }

        private void FilterList()
        {
            if (_cachedTypes == null)
            {
                _filteredTypes = new List<Type>();
                return;
            }

            var s = _search?.Trim();
            if (string.IsNullOrEmpty(s))
            {
                _filteredTypes = _cachedTypes;
                return;
            }

            s = s.ToLowerInvariant();

            _filteredTypes = _cachedTypes
                .Where(t => 
                    (t.FullName != null && t.FullName.ToLowerInvariant().Contains(s)) ||
                    (t.Name.ToLowerInvariant().Contains(s)))
                .ToList();
        }
    }
}