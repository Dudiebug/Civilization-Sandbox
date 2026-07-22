using CivSandbox.UI;
using CivSandbox.World;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class WorldCreationScreenTests
    {
        [Test]
        public void RuntimeWorldCreatorOwnsAThemedDocumentAndSetupPanel()
        {
            var screenObject = new GameObject("World creation UI test");
            try
            {
                WorldCreationScreen screen = screenObject.AddComponent<WorldCreationScreen>();
                screen.Initialize(_ => { }, (_, __) => { }, () => { });
                UIDocument document = screenObject.GetComponentInChildren<UIDocument>(true);

                Assert.That(document, Is.Not.Null);
                Assert.That(document.panelSettings, Is.Not.Null);
                Assert.That(document.panelSettings.themeStyleSheet, Is.Not.Null);
                Assert.That(document.rootVisualElement.styleSheets.count, Is.GreaterThan(0));
                Assert.That(document.rootVisualElement.Q<VisualElement>("WorldSetupPanel"), Is.Not.Null);
                Assert.That(document.rootVisualElement.Q<Button>("RandomizeSeedButton"), Is.Not.Null);
                ulong previousSeed = screen.CurrentSeed;
                screen.RandomizeSeed();
                Assert.That(screen.CurrentSeed, Is.Not.EqualTo(previousSeed));
                Assert.That(screen.CanPickWorld(Vector3.zero), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(screenObject);
            }
        }
    }
}
