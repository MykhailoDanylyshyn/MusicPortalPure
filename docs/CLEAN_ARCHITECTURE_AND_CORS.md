# Чиста архітектура, окремий Web API та SPA з CORS

## Структура рішення

- `MusicPortal.Domain` — сутності предметної області. Не має посилань на інші проєкти.
- `MusicPortal.Application` — DTO, сценарії/сервіси, інтерфейси сервісів, `IRepository` та `IUnitOfWork`. Залежить лише від Domain.
- `MusicPortal.Infrastructure` — EF Core, SQL Server, DbContext, міграції та реалізації репозиторіїв. Залежить від Application і Domain.
- `MusicPortal.WebApi` — окремий REST API host. Контролери, автентифікація, DI та CORS.
- `MusicPortal.AdminSPA` — окремий статичний вебклієнт. HTML/CSS/JavaScript і Fetch API.
- `MusicPortal` — попередній MVC-портал (реєстрація, вхід, каталог). Він використовує Application та Infrastructure як composition root.

Напрям залежностей: `WebApi -> Application <- Infrastructure`, а `Application -> Domain`; Infrastructure також посилається на Domain. Domain не залежить від інших шарів.

## Адреси для локального запуску

- REST API: `https://localhost:7001`
- SPA: `https://localhost:7202`
- MVC портал (за потреби): `https://localhost:7081`

API та SPA працюють на різних origins (схема/host/port), тому браузер застосовує CORS.

## Налаштування CORS

У `MusicPortal.WebApi/appsettings.json` параметр `SpaOrigin` має точно збігатися з origin SPA:

```json
"SpaOrigin": "https://localhost:7202"
```

У `Program.cs` API дозволяє саме цей origin, HTTP-заголовки, методи та credentials. `UseCors` розташований після `UseRouting` і до `UseAuthentication`/`UseAuthorization`.

У `MusicPortal.AdminSPA/wwwroot/js/api-config.js` задається API base URL:

```javascript
window.MUSIC_PORTAL_API_BASE_URL = 'https://localhost:7001';
```

Fetch використовує `credentials: 'include'`, оскільки API видає HttpOnly cookie після входу адміністратора. API cookie має `SameSite=None` та `Secure` для cross-origin запитів. Обидва localhost origins потрібно відкривати через HTTPS.

> Для розгортання на різних реальних доменах потрібно вказати production SPA origin в API configuration і production API URL у `api-config.js`. Браузери можуть блокувати third-party cookies між різними сайтами; для такого deployment краще перейти на Bearer/JWT або розмістити клієнт та API в межах одного site. CORS сам по собі не є автентифікацією.

## Підготовка

1. Переконайтеся, що SQL Server запущений, а `MusicPortal.WebApi/appsettings.json` та `MusicPortal/appsettings.json` містять правильний `DefaultConnection`.
2. У Visual Studio відкрийте `MusicPortal.slnx` і виконайте Restore NuGet Packages.
3. Виконайте Build → Rebuild Solution.
4. Якщо локальний HTTPS сертифікат ще не довірений, у терміналі виконайте `dotnet dev-certs https --trust`.
5. Для першого адміністратора зареєструйте користувача через MVC-портал, потім виконайте `docs/bootstrap-admin.sql`, замінивши `admin_username` на його логін.

## Запуск і перевірка

1. У Visual Studio відкрийте властивості Solution → Startup Projects → Multiple startup projects.
2. Встановіть `Start` для `MusicPortal.WebApi` і `MusicPortal.AdminSPA`. (MVC `MusicPortal` запускайте окремо, коли потрібно зареєструвати користувача або користуватися основним порталом.)
3. Відкрийте SPA за адресою `https://localhost:7202`.
4. Увійдіть під адміністратором. API видасть HttpOnly cookie; JavaScript не читає cookie напряму.
5. Перевірте вкладки Users, Artists, Genres: читання списків — GET; додавання — POST; редагування — PUT; видалення — DELETE. Підтвердження реєстрації користувача — додатковий PATCH.
6. Відкрийте DevTools → Network. У запитах до `https://localhost:7001` перевірте `Origin: https://localhost:7202`, CORS response headers та методи запитів. Для preflight-запитів OPTIONS відповідь має містити `Access-Control-Allow-Origin` і `Access-Control-Allow-Credentials: true`.
7. Для перевірки захисту вийдіть зі SPA та повторіть API-запит: очікується HTTP 401. Увійдіть звичайним користувачем: API login повертає 403, оскільки панель призначена адміністраторам.

## Основні API маршрути

- `POST /api/auth/login`, `POST /api/auth/logout`
- `GET /api/users`, `GET /api/users/{id}`, `POST /api/users`, `PUT /api/users/{id}`, `DELETE /api/users/{id}`
- `PATCH /api/users/{id}/approval`
- `GET /api/artists`, `GET /api/artists/{id}`, `POST /api/artists`, `PUT /api/artists/{id}`, `DELETE /api/artists/{id}`
- `GET /api/genres`, `GET /api/genres/{id}`, `POST /api/genres`, `PUT /api/genres/{id}`, `DELETE /api/genres/{id}`

## Що показати викладачу

1. Solution Explorer з п'ятьма окремими шарами/застосунками (Domain, Application, Infrastructure, WebApi, AdminSPA) та legacy MVC host.
2. `MusicPortal.Application` не має ProjectReference на Infrastructure або WebApi.
3. `MusicPortal.Infrastructure` реалізує Application інтерфейси репозиторіїв.
4. Два окремі URL: SPA `:7202`, API `:7001`.
5. Network tab з Fetch-запитами GET/POST/PUT/DELETE і CORS заголовками.
6. Результат входу адміністратора та відмову доступу для неадміністратора.
