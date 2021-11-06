using Zenject;
using Game.Core;
using Game.Core.Sounds;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public ScoreManager ScoreManager;
        public SoundManager SoundManager;
        public TimeManager TimeManager;

        public override void InstallBindings()
        {
            Container.Bind<PrefabFactory>().AsSingle();
            
            Container.Bind<SoundManager>().FromInstance(SoundManager).AsSingle();
            Container.Bind<ScoreManager>().FromInstance(ScoreManager).AsSingle();
            Container.Bind<TimeManager>().FromInstance(TimeManager).AsSingle();
        }
    }
}