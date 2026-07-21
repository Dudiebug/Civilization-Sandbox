using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;

namespace CivSandbox.Tooling.Tests
{
    public sealed class BootstrapProjectTests
    {
        [Test]
        public void PinnedToolchainMatchesProject()
        {
            string root = Directory.GetParent(Application.dataPath)!.FullName;
            string version = File.ReadAllText(Path.Combine(root, "ProjectSettings", "ProjectVersion.txt"));
            string manifest = File.ReadAllText(Path.Combine(root, "Packages", "manifest.json"));
            string contract = File.ReadAllText(Path.Combine(root, "Config", "toolchain.json"));
            string lockPath = Path.Combine(root, "Packages", "packages-lock.json");

            StringAssert.Contains("6000.3.20f1 (c9ba695d4f07)", version);
            var expectedPins = new Dictionary<string, string>
            {
                ["com.unity.burst"] = "1.8.29",
                ["com.unity.collections"] = "2.6.8",
                ["com.unity.entities"] = "1.4.8",
                ["com.unity.entities.graphics"] = "1.4.21",
                ["com.unity.render-pipelines.universal"] = "17.3.0",
                ["com.unity.sdk.linux-x86_64"] = "1.1.0",
                ["com.unity.test-framework"] = "1.6.0",
                ["com.unity.toolchain.win-x86_64-linux"] = "1.1.0",
                ["com.civsandbox.tooling"] = "file:../GamePackages/com.civsandbox.tooling"
            };
            foreach (KeyValuePair<string, string> pin in expectedPins)
            {
                StringAssert.Contains($"\"{pin.Key}\": \"{pin.Value}\"", manifest, $"Missing direct package pin {pin.Key}.");
            }

            Assert.That(File.Exists(lockPath), Is.True, "Committed package lock is required.");
            using var stream = File.OpenRead(lockPath);
            using var sha = SHA256.Create();
            string actualHash = System.BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", string.Empty).ToLowerInvariant();
            StringAssert.Contains($"\"packageLockSha256\": \"{actualHash}\"", contract);
        }
    }
}
