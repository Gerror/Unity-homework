using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game;
using Input;
using UnityEngine.SceneManagement;

namespace PlayModeTests
{
    public class TestsZombieBot
    {
        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("Scenes/Game");
        }

        [UnityTest]
        public IEnumerator TestsZombieBotWithEnumeratorPasses()
        {
            var playerController = Object.FindObjectOfType<PlayerController>();
            var zombie = Object.FindObjectOfType<ZombieBotInput>();
            var gameView = Object.FindObjectOfType<GameView>();

            playerController.PlayerInput = null;

            playerController.gameObject.transform.position = zombie.gameObject.transform.position +
                                                             zombie.gameObject.transform.forward;
            yield return new WaitForSeconds(1f);
            Assert.NotNull(zombie.Target);
            
            yield return new WaitForSeconds(5f);
            Assert.True(gameView.IsGameOver);
            Assert.False(gameView.PlayerWin);
        }
    }
}