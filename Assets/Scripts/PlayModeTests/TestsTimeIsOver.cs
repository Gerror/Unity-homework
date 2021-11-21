using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game;
using UnityEngine.SceneManagement;

namespace PlayModeTests
{
    public class TestsTimeIsOver
    {
        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("Scenes/Game");
        }

        [UnityTest]
        public IEnumerator TestsTimeIsOverWithEnumeratorPasses()
        {
            var playerController = Object.FindObjectOfType<PlayerController>();
            var gameView = Object.FindObjectOfType<GameView>();

            playerController.PlayerInput = null;
            
            yield return new WaitForSeconds(playerController.Hitpoints + 1);
            Assert.True(gameView.IsGameOver);
            Assert.False(gameView.PlayerWin);
        }
    }
}