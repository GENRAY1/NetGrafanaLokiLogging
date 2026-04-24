# NetGrafanaLokiLogging

Локальный стенд с:
- ASP.NET backend
- Grafana
- Loki
- MinIO (S3-совместимое хранилище для Loki)

## 1. Подготовка env

1. Проверьте файл `.env`
2. При необходимости измените логин/пароль и порты.

## 2. Запуск

```bash
docker compose up -d --build
```

Сервисы:
- Backend: `http://localhost:5000`
- Grafana: `http://localhost:3000`
- Loki API: `http://localhost:3100`
- MinIO API: `http://localhost:9000`
- MinIO Console: `http://localhost:9001`

## 3. Grafana для мониторинга логов

Вход

1. Перейдите по адресу http://localhost:3000/login
2. Введите данные (по дефолту username: admin, pass: admin)

После входа в Grafana нужно вручную добавить источник данных:

1. Перейдите в `Connections` => `Data sources`
2. Нажмите `Add data source`
3. Выберите `Loki`
4. Заполните поля:
   - `Connection url`: `loki:3100`
   - `Authentication`: `No Authentication`
5. Нажмите `Save & test`

