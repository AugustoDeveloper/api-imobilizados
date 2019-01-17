
create table if not exists public.floor (
	id bigint  not null generated always as identity,
	"level" int4 not null,
	levelname varchar(150) not null,
	constraint floor_pkey primary key (id)
);

create table if not exists public.hardware (
	id bigint not null generated always as identity,
	"name" varchar(200) not null,
	brand varchar(150) null,
	factorycode varchar(150) null,
	description text null,
	isimmobilized bit(1) null default 0::bit(1),
	floorid bigint null references floor(id),
	constraint hardware_key primary key(id)
);