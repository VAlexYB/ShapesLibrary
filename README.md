В базе данных MS SQL Server есть продукты и категории. Одному продукту может соответствовать много категорий, в одной категории может быть много продуктов. Напишите SQL запрос для выбора всех пар «Имя продукта – Имя категории». Если у продукта нет категорий, то его имя все равно должно выводиться.

```sql
select 
  p.Name as ProductName,
  c.Name as CategoryName
from
  Products p
left join 
  ProductCategories pc on p.Id = pc.ProductId
left join 
  Categories c on pc.CategoryId = c.Id
```
Опишите решение для веб-приложения в kubernetes в виде yaml-манифеста. Оставляйте в коде комментарии по принятым решениям. Есть следующие вводные:

У нас kubernetes кластер, в котором пять нод.
Приложение испытывает постоянную стабильную нагрузку в течение суток без значительных колебаний. 3 пода справляются с нагрузкой.
На первые запросы приложению требуется значительно больше ресурсов CPU, в дальнейшем потребление ровное в районе 0.1 CPU. По памяти всегда “ровно” в районе 128M memory.
Приложение требует около 5-10 секунд для инициализации.
Что хотим?

Минимальное потребление ресурсов от этого deployment’а.
Размещение подов на разных нодах для отказоустойчивости.
Чтобы под не обрабатывал запросы до завершения инициализации.
```yml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: my-app-deployment
  labels:
    app: my-app
spec:
  replicas: 3  # Запускаем 3 пода для стабильной нагрузки
  selector:
    matchLabels:
      app: my-app
  template:
    metadata:
      labels:
        app: my-app
    spec:
      containers:
      - name: my-app-container
        image: my-app-image:latest
        ports:
        - containerPort: 80 
        resources:
          requests:
            memory: "128Mi"  # Минимальный запрос по памяти
            cpu: "100m"      # Потребление в стабильно ровное время
          limits:
            memory: "256Mi"  # Ограничение памяти, если потребуется больше во время нагрузки
            cpu: "500m"      # Ограничение CPU для первых запросов
        readinessProbe:  # Пробка Readiness для отказа от обработки запросов до полной готовности
          httpGet:
            path: /healthz  # Путь проверки состояния 
            port: 80
          initialDelaySeconds: 10  # Подождать 10 секунд перед первой проверкой
          periodSeconds: 5         # Проверять каждые 5 секунд после этого
        livenessProbe:  # Пробка Liveness для проверки работоспособности
          httpGet:
            path: /healthz  # Может использовать тот же путь
            port: 80
          initialDelaySeconds: 30  # Даем больше времени на инициализацию
          periodSeconds: 10        # Проверка каждые 10 секунд
        startupProbe:  # Пробка Startup для проверки успешной инициализации (важно для долгой инициализации)
          httpGet:
            path: /healthz
            port: 80
          failureThreshold: 12  # Максимум 12 попыток (12 * 5 = 60 секунд на запуск)
          periodSeconds: 5

      affinity:  # Задаем правила для распределения подов по нодам
        podAntiAffinity:  # Размещение подов на разных нодах для отказоустойчивости
          requiredDuringSchedulingIgnoredDuringExecution:
            - labelSelector:
                matchExpressions:
                  - key: app
                    operator: In
                    values:
                      - my-app
              topologyKey: "kubernetes.io/hostname"  # Запрещаем запускать поды на одной и той же ноде

  strategy:  # Описываем стратегию развертывания
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1  # Максимум 1 новый под во время обновления
      maxUnavailable: 0  # Все поды должны быть доступны во время обновления
