using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Game;
using Search;
using UnityEngine.SceneManagement;

namespace PlayModeTests
{
    public class TestsPlayerWin
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
            var gameSettings = Object.FindObjectOfType<GameSettings>();

            gameSettings.Difficalty = Difficalty.Easy;

            var aStarBot = playerController.gameObject.GetComponent<AStarBot>();
            
            playerController.PlayerInput = aStarBot;
            playerController.FireTime = 0.5f;

            for (int i = 0; i < playerController.Hitpoints + 1; i++)
            {
                if (gameView.IsGameOver)
                {
                    Assert.True(gameView.IsGameOver);
                    Assert.True(gameView.PlayerWin);
                    break;
                }
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
