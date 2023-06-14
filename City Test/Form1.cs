using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;


namespace City_Test
{   //Модуль 1, начало 
    public partial class CityTest : Form
    {
        // Размеры изображения, из которого создается дорога
        private const int ROAD_SIZE = 32;

        // Модуль 3, начало Создание подключение к базе данных SQLite
        private SQLiteConnection _connection = new SQLiteConnection("Data Source=database.db");
        private SQLiteCommand _command = new SQLiteCommand();

        // Ссылки на изображения для создания дорог
        private Bitmap[] _roadBitmaps = new Bitmap[]
        {
            null,
            Properties.Resources.Rvertical, //дорога вверх
            Properties.Resources.Rvertical,
            Properties.Resources.Rvertical,
            Properties.Resources.Rvertical,
            Properties.Resources.Rcrossroads, //перекресток
            Properties.Resources.Rstart, // Старт
            Properties.Resources.Rfinish // Финиш
        };

        // Изображение автомобиля
        private Bitmap[] _carBitmaps = new Bitmap[]
        {
            Properties.Resources.Cvertical1, //движение вверх
            Properties.Resources.Cright, // движение направо
            Properties.Resources.Cbottom, //движение вниз
            Properties.Resources.Cleft //движение налево
        };

        // Массив дороги
        private int[,] _roadmap = {
            {0,2,1,0,0,0,2,1,0,0,0,0,2,1,0,0,0,0,0,0 },
            {0,2,1,0,0,0,2,1,0,0,0,0,2,1,0,0,0,0,0,0 },
            {0,5,5,4,4,4,5,5,4,4,4,4,5,5,4,4,5,5,4,4 },
            {0,5,5,3,3,3,5,5,3,3,3,3,5,5,3,3,5,5,3,3 },
            {0,2,1,0,0,0,2,1,0,0,0,0,2,1,0,0,2,1,0,0 },
            {7,5,5,0,0,0,2,1,0,0,0,0,2,1,0,0,2,1,0,0 },
            {3,5,5,0,0,0,5,5,4,4,4,4,5,5,0,0,2,1,0,0 },
            {0,2,1,0,0,0,5,5,3,3,3,3,5,5,0,0,2,1,0,0 },
            {0,2,1,0,0,0,2,1,0,0,0,0,5,5,4,4,5,5,4,4 },
            {0,5,5,4,4,4,5,5,0,0,0,0,5,5,3,3,5,5,3,3 },
            {0,5,5,3,3,3,5,5,0,0,0,0,2,1,0,0,2,1,0,0 },
            {0,2,1,0,0,0,5,5,4,4,4,4,5,5,0,0,2,1,0,0 },
            {0,2,1,0,0,0,5,5,3,3,3,3,5,5,0,0,2,1,0,0 },
            {0,2,1,0,0,0,2,1,0,0,0,0,2,1,0,0,2,1,0,0 },
            {0,2,1,0,0,0,2,1,0,0,0,0,2,1,0,0,2,1,0,0 },
            {0,2,1,0,0,0,2,1,0,0,0,0,2,1,0,0,2,1,0,0 },
            {4,5,5,4,4,4,5,5,4,4,4,4,5,5,4,4,5,5,0,0 },
            {3,5,5,3,3,3,5,5,3,3,3,3,5,5,3,3,5,5,0,0 },
            {0,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,0,0 },
            {0,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2,6,0,0 },
        };

        private int[,] _objects = {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0 },
            {0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0 },
            {0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        };

        // Координаты выделенного элемента дороги
        private int[] _selectedRoadPosition = new int[] { -1, -1 };

        // Расположение автомобиля
        private int[] _carPosition = new int[] { 17, 19, 0 };

        // Обучение 
        private int[] _lastCross = new int[2];
        private int _iteration = 0;
        private int _step = 0;
        private int _bestResult = 0;

        public CityTest()
        {
            InitializeComponent();

            // Внесение записи в базу данных, создание таблицы, если таковая отсутствует
            _connection.Open();
            _command.Connection = _connection;
            _command.CommandText = "CREATE TABLE IF NOT EXISTS Logs (Log_Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Log_result varchar(6) NOT NULL, Log_time INTEGER NOT NULL)";
            _command.ExecuteNonQuery();
        }

        private void CityTest_Load(object sender, EventArgs e)
        {
            TransposeArrays(); // Необходимо перевернуть массивы
            BuildRoad(); // Сборка дорожной карты
            DrawRoad(); // Отрисовка объектов и дороги
        }

        private void TransposeArrays()
        {
            int[,] roadmap = new int[20, 20];
            int[,] objects = new int[20, 20];
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    roadmap[x, y] = _roadmap[y, x];
                    objects[x, y] = _objects[y, x];
                }
            }
            _roadmap = roadmap;
            _objects = objects;
        }

        private void BuildRoad()
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    // Создание изображения дороги
                    PictureBox road = new PictureBox
                    {
                        Name = $"{x}_{y}", // Имя
                        Size = new Size(ROAD_SIZE, ROAD_SIZE), // Размеры
                        Location = new Point(ROAD_SIZE * x, ROAD_SIZE * y), // Расположние
                    };

                    // Привязка событий
                    road.Click += RoadClick; // Выделение при нажатии
                    road.MouseHover += RoadHover; // Отображение координат элемента дороги

                    // Заполение панели изображениями 
                    panel.Controls.Add(road);
                }
            }
        }

        private void DrawRoad()
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    // Доступ к изображению дороги
                    PictureBox road = panel.Controls[x + "_" + y] as PictureBox;
                    road.BackgroundImage = _roadBitmaps[_roadmap[x, y]];

                    // Поворот изображения
                    switch (_roadmap[x, y])
                    {
                        case 2:
                            road.BackgroundImage = RotateImage(road.BackgroundImage, 180);
                            break;
                        case 3:
                            road.BackgroundImage = RotateImage(road.BackgroundImage, 90);
                            break;
                        case 4:
                            road.BackgroundImage = RotateImage(road.BackgroundImage, 270);
                            break;
                         
                    }

                    // Установка знака "движения запрещено" и сброс движения автомобиля
                    switch (_objects[x, y])
                    {
                        case 1:
                            road.Image = Properties.Resources.Rblock;
                            break;
                        case 2:
                            road.Image = Properties.Resources.Cvertical1;
                            break;
                    }
                }
            }
        }

        // Поворот изображения
        private Image RotateImage(Image image, float angle)
        {
            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(image.Width / 2, image.Height / 2);
                g.RotateTransform(angle);
                g.DrawImage(image, -image.Width / 2, -image.Height / 2, image.Width, image.Height + 1);
            }
            return rotatedImage;
        }

        //Выделение элемента дороги
        private void RoadClick(object sender, EventArgs e)
        {
            PictureBox road = sender as PictureBox;
            string[] roadPosition = road.Name.Split('_');
            int x = int.Parse(roadPosition[0]);
            int y = int.Parse(roadPosition[1]);

            // Удаление рамок на предыдущей выделенной дороге
            if (_selectedRoadPosition[0] > 0)
            {
                PictureBox selectedRoad = panel.Controls[_selectedRoadPosition[0] + "_" + _selectedRoadPosition[1]] as PictureBox;
                selectedRoad.BorderStyle = BorderStyle.None;
            }

            // Выделение дороги
            road.BorderStyle = BorderStyle.Fixed3D;
            button_add.Enabled = true;
            _selectedRoadPosition[0] = x;
            _selectedRoadPosition[1] = y;
            if (_objects[_selectedRoadPosition[0], _selectedRoadPosition[1]] == 1)
            {
                button_delete.Enabled = true;
            }
            else
            {
                button_delete.Enabled = false;
            }
        }

        private void RoadHover(object sender, EventArgs e)
        {
            // Отображение координат
            PictureBox road = sender as PictureBox;
            label_info.Text = "Координаты выделенной зоны: " + road.Name;
        }

        //Создание кнопки установки знака "Движение запрещено"
        private void button_add_Click(object sender, EventArgs e)
        {
            int x = _selectedRoadPosition[0];
            int y = _selectedRoadPosition[1];

            if (x != -1)
            {
                if (_roadmap[x, y] >= 1 & _roadmap[x, y] <= 4)
                {
                    PictureBox selectedRoad = panel.Controls[x + "_" + y] as PictureBox;
                    selectedRoad.Image = Properties.Resources.Rblock;
                    _objects[x, y] = 1;
                }
            }
        }

        //Создание кнопки удаления знака "Движение запрещено"
        private void button_delete_Click(object sender, EventArgs e)
        {
            int x = _selectedRoadPosition[0];
            int y = _selectedRoadPosition[1];

            if (x != -1)
            {
                PictureBox road = panel.Controls[x + "_" + y] as PictureBox;
                road.Image = null;
                _objects[x, y] = 0;
                button_delete.Enabled = false;
            }
        }
     //Модуль 1, конец

        //Модуль 2, начало
        
        //Создание кнопок ручного управления автомобилем
        private void button_up_Click(object sender, EventArgs e)
        {
            MoveCar();
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            RotateCar(-1);
        
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            RotateCar(2);
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            RotateCar(1);
        }

        private void RotateCar(int rotate)
        {
            int x = _carPosition[0];
            int y = _carPosition[1];
            int direction = _carPosition[2];

            direction = RotateRight(direction, rotate);
            _carPosition[2] = direction;

            PictureBox road = panel.Controls[x + "_" + y] as PictureBox;
            road.Image = _carBitmaps[direction];
        }
        //Задаем правила движения автомобиля
        private void MoveCar()
        {
            // Координаты автомобиля
            int x = _carPosition[0];
            int y = _carPosition[1];
            int direction = _carPosition[2];

            // Текущая дорога
            PictureBox roadStart = panel.Controls[x + "_" + y] as PictureBox;

            // Проверяем следующие координаты
            ChangePosition(direction, ref x, ref y);

            // Проверяем существует ли возможность двигаться прямо
            if (x < 20 && y < 20 && x >= 0 && y >= 0)
            {
                // Проверка наличия знака "движение запрещено" на дороге
                if (_roadmap[x, y] != 0 && _objects[x, y] == 0 && _roadmap[x, y] != 6)
                {
                    // Получаем изображения следующего элемента дороги
                    PictureBox roadEnd = panel.Controls[x + "_" + y] as PictureBox;

                    // Обновляем положение изображения
                    roadStart.Image = null;
                    roadEnd.Image = _carBitmaps[direction];

                    // Записываем координаты выезда с последнего прекрестка
                    if (_roadmap[_carPosition[0], _carPosition[1]] == 5)
                    {
                        if (_roadmap[x, y] != 5)
                        {
                            _lastCross[0] = x;
                            _lastCross[1] = y;
                        }
                    }

                    // Обновляем координаты
                    _carPosition = new int[] { x, y, direction };

                    // Проверяем на наличие финиша
                    if (_roadmap[x, y] == 7)
                    {
                        if (_bestResult == 0 || _step < _bestResult)
                        {
                            _bestResult = _step;
                            label_best.Text = "Лучший проход = " + _bestResult.ToString();
                        }
                        StopMove(true);
                    }
                }
                else
                {
                    // Если машина на перекрестке
                    if (_roadmap[_carPosition[0], _carPosition[1]] == 5)
                    {
                        // Попытка повернуть направо
                        x = _carPosition[0];
                        y = _carPosition[1];
                        direction = RotateRight(direction, 1);
                        ChangePosition(direction, ref x, ref y);
                        // ChangePosition(direction, ref x, ref y);

                        if (CompareRotation(direction, _roadmap[x, y]))
                        {
                            _carPosition[2] = direction;
                            roadStart.Image = _carBitmaps[direction];
                        }
                        else
                        {
                            // Попытка повернуть налево
                            direction = RotateRight(direction, 2);
                            x = _carPosition[0];
                            y = _carPosition[1];
                            ChangePosition(direction, ref x, ref y);
                            ChangePosition(direction, ref x, ref y);

                            if (CompareRotation(direction, _roadmap[x, y]))
                            {
                                _carPosition[2] = direction;
                                roadStart.Image = _carBitmaps[direction];
                            }
                            else
                            {
                                x = _carPosition[0];
                                y = _carPosition[1];

                                StopMove(false);
                                SetBlock(x, y);
                            }
                        }
                    }
                    else
                    {
                        StopMove(false);
                        SetBlockOnCross();
                    }
                }
            }
            else
            {
                StopMove(false);
                SetBlockOnCross();
            }
        }

        private bool CompareRotation(int directionCar, int directionRoad)
        {
            switch (directionCar)
            {
                case 0:
                    return (directionRoad == 1 || directionRoad == 5);
                case 1:
                    return (directionRoad == 3 || directionRoad == 5);
                case 2:
                    return (directionRoad == 2 || directionRoad == 5 || directionRoad == 7);
                case 3:
                    return (directionRoad == 4 || directionRoad == 5 || directionRoad == 7);
            }
            return false;
        }

        private void ChangePosition(int direction, ref int x, ref int y)
        {
            // 0 - Вверх
            // 1 - Вправо
            // 2 - Вниз
            // 3 - Влево
            switch (direction)
            {
                case 0:
                    y--;
                    break;
                case 1:
                    x++;
                    break;
                case 2:
                    y++;
                    break;
                case 3:
                    x--;
                    break;
            }
        }

        private int RotateRight(int direction, int count)
        {
            direction += count;
            while (direction >= 4)
            {
                direction -= 4;
            }

            while (direction < 0)
            {
                direction += 4;
            }

            return direction;
        }
        //Модуль 2, конец 

        //Модуль 4, начало
        //Создание таймера и кнопок начала, конца обучения

        private void button_start_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void button_end_Click(object sender, EventArgs e)
        {
            timer.Stop();
            StopMove(false);
        }

        private void StopMove(bool result)
        {
            // Удаление автомобиля
            PictureBox car = panel.Controls[_carPosition[0] + "_" + _carPosition[1]] as PictureBox;
            car.Image = null;
            _carPosition = new int[3] { 17, 19, 0 };

            // Установка автомобиля на старт
            car = panel.Controls[_carPosition[0] + "_" + _carPosition[1]] as PictureBox;
            car.Image = Properties.Resources.Cvertical1;

            // Запись результата в БД
            _command.CommandText = "INSERT INTO Logs (Log_result, Log_time) VALUES ('" + result + "', " + _step + ")";
            _command.ExecuteNonQuery();
        //Модуль 3, конец
            _iteration++;
            label_iteration.Text = "Итерация: " + _iteration.ToString();
            _step = 0;
        }

        //Создание полосы для ускорения анимации движения автомобиля
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            timer.Interval = (10 - trackBar.Value) * 100;
            label_speed.Text = "Частота обновления (мс): " + timer.Interval + "мс";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            MoveCar();
            _step++;
            label_step.Text = "Шагов за итерацию: " + _step.ToString();
        }
        //Модуль 4, конец

        //Доп блоки для ускорения процесса обучения
        private void SetBlockOnCross()
        {
            _roadmap[_lastCross[0], _lastCross[1]] = 6;
            PictureBox road = panel.Controls[_lastCross[0] + "_" + _lastCross[1]] as PictureBox;
            road.Image = Properties.Resources.Block;
        }

        private void SetBlock(int x, int y)
        {
            _roadmap[x, y] = 6;
            PictureBox road = panel.Controls[x + "_" + y] as PictureBox;
            road.Image = Properties.Resources.Block;
        }


    }
}