# PushPush Game Server

PushPush 게임용 ASP.NET Core 기반 게임 서버입니다. 계정 생성/로그인, 세션 발급, 유저 정보 및 아이템 조회 API를 제공합니다.

## 기술 스택

| 구분 | 사용 기술 |
| --- | --- |
| 런타임 | .NET 10 / ASP.NET Core Web API |
| DB | MySQL 8.4 (Pomelo EF Core Provider) |
| 세션 저장소 | Redis 7 (StackExchange.Redis) |
| ORM | Entity Framework Core 9 |
| API 문서 | OpenAPI (개발 환경 한정) |

## 프로젝트 구조

```
PushPushServer/
├── Controllers/         API 엔드포인트
│   ├── LoginController      계정 생성 / 로그인
│   ├── UserController       유저 정보 조회
│   ├── ItemController       보유 아이템 조회
│   └── ScoreController      점수 처리 (구현 예정)
├── Services/            비즈니스 로직
│   ├── UserService          유저 생성/조회, 중복키 재시도 처리
│   ├── ItemService          아이템 조회
│   ├── SessionService       Redis 토큰 발급/검증
│   └── FriendCodeGenerator  친구코드 생성 (혼동 문자 제외 8자리)
├── Data/                EF Core DbContext 및 엔티티 설정
├── Models/              DB 엔티티 (User, Item)
├── DTO/                 요청/응답 모델, 공통 응답 및 결과 코드
├── Migrations/          EF Core 마이그레이션
└── Program.cs           DI 등록 및 파이프라인 구성
```

## 설계 특징

### 공통 응답 규약

모든 응답은 `BaseResponse`를 상속하며, HTTP 상태 코드가 아닌 `ResultCode`로 성공/실패를 구분합니다. 모델 검증 실패와 처리되지 않은 예외도 동일한 형태로 변환되어 반환됩니다.

```json
{
  "resultCode": 0,
  "message": null,
  "token": "A1B2C3..."
}
```

| 코드 | 값 | 의미 |
| --- | --- | --- |
| `Success` | 0 | 성공 |
| `InvalidRequest` | 1 | 요청 형식/검증 오류 |
| `ServerError` | 2 | 서버 내부 오류 |
| `Unauthorized` | 3 | 토큰 없음 또는 만료 |
| `UserNotFound` | 1001 | 계정 없음 |
| `DuplicateUserId` | 1002 | 중복된 계정 ID |
| `CreateUserFail` | 1003 | 계정 생성 실패 |

### 세션 관리

로그인 시 32바이트 난수 토큰을 발급하고 Redis에 24시간 TTL로 저장합니다. 동일 계정으로 재로그인하면 이전 토큰을 삭제해 중복 로그인을 차단합니다.

```
session:{token}     -> uid
user:{uid}:session  -> token
```

### 계정 생성 동시성 처리

계정 생성 시 친구코드 충돌 또는 동일 ID 동시 요청으로 인한 중복키 예외를 감지해 최대 5회까지 재시도합니다. 동일 ID가 먼저 생성된 경우에는 해당 유저를 그대로 반환합니다.

## API

모든 엔드포인트는 `POST`이며 기본 경로는 `/api` 입니다.

### `POST /api/login` — 로그인

```json
{ "userId": "testuser01" }
```

`userId`는 4~10자의 영문/숫자/`_` 조합이어야 합니다. 성공 시 세션 토큰을 반환합니다.

### `POST /api/login/create` — 계정 생성

```json
{ "userId": "testuser01", "nickName": "테스터" }
```

`nickName`은 2~8자의 한글/영문/숫자/`_` 조합입니다. 생성 후 바로 토큰을 발급합니다.

### `POST /api/user/getuserinfo` — 유저 정보 조회

```json
{ "token": "발급받은_토큰" }
```

닉네임, 친구코드, 경험치를 반환합니다.

### `POST /api/item/getitems` — 보유 아이템 조회

```json
{ "token": "발급받은_토큰" }
```

`{ uid, dataId, amount }` 형태의 아이템 목록을 반환합니다.

## 실행 방법

### 1. 의존 서비스 기동

```bash
docker compose up -d
```

MySQL(3306)과 Redis(6379)가 함께 실행됩니다.

### 2. DB 마이그레이션 적용

```bash
cd PushPushServer
dotnet ef database update
```

### 3. 서버 실행

```bash
dotnet run
```

기본 주소는 `http://localhost:5241`, HTTPS 프로필은 `https://localhost:7185` 입니다. 개발 환경에서는 `/openapi/v1.json` 으로 API 스펙을 확인할 수 있습니다.

### 연결 문자열

`appsettings.Development.json`의 `ConnectionStrings` 항목에서 `GameDB`와 `Redis` 주소를 설정합니다. 운영 환경에서는 환경 변수나 시크릿 관리자를 통해 주입하세요.

## 요청 예시

[`PushPushServer.http`](PushPushServer/PushPushServer.http) 파일에 바로 실행 가능한 요청 샘플이 포함되어 있습니다.

## 개발 현황

- [x] 계정 생성 / 로그인
- [x] Redis 기반 세션 관리
- [x] 유저 정보 조회
- [x] 아이템 조회
- [ ] 점수 등록 (`ScoreController` 스텁 상태)

## 라이선스

[MIT](LICENSE)
