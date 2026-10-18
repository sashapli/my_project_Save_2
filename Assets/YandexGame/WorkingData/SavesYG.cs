
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        public bool isFirstSessions = true;

        public int Distan;
        public int coins = 0;
        public int bakegears= 0;
        public int Forwardgears = 0;
        public bool ISArm = false;
        public bool ISWheel = false;
        public int bestDistance = 0;
        public int[] BikeMe = { 1, 0, 0, 0, 0 };
        public int BikeEcp = 0;
        public char gas = 'd';
        public char bake = 'a';
        public char right = 'e';
        public char left = 'q';
        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
