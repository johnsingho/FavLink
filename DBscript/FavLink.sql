
-- TODO create FavLink Databse first

USE FavLink;
-- use FavLink_Test;

create table tbl_user (
   id                   int                  identity,
   ADAccount            varchar(50)          not null,
   FullName             varchar(50)          not null,
   Email                varchar(100)         not null,
   LastLogon            datetime             null,
   IsValid              bit        not null default(1),
   IsAdmin              bit        not null default(0),
   constraint PK_TBL_USER primary key (id)
)
go

-- 分类
create table tbl_category (
   id                   int                  identity,
   name                 nvarchar(20)         not null,
   constraint PK_TBL_CATEGORY primary key (id)
)
go


-- IT人员表
create table tbl_itsupport (
   id                   int                  identity,
   name                 nvarchar(20)         not null,
   phone_number         varchar(20)          null,
   constraint PK_TBL_ITSUPPORT primary key (id)
)
go

-- IT人员排班表
/*
shift
   0 - Morning Shift
   1 - Middle Shift
   2 - Night Shift
*/
create table tbl_itsupport_arrangment (
   id                   int                  identity,
   ref_personID         int                  not null,
   project              nvarchar(100)        null,
   shift                int                  null,
   month                int                  null,
   constraint PK_TBL_ITSUPPORT_ARRANGMENT primary key (id)
)
go


-- 用户的所有链接数据
create table tbl_link_data (
   id                   int                  identity,
   ref_userID           int                  null,
   name                 nvarchar(50)         null,
   url                  varchar(100)         null,
   ref_categoryID       int                  null,
   icon                 varchar(20)          null,
   bg_color             varchar(20)          null,
   constraint PK_TBL_LINK_DATA primary key (id)
)
go

-- category, 1 for Hotline
-- 2 for Escalation
create table tbl_hotline (
   id                   int                  identity,
   name                 nvarchar(20)         not null,
   phone_number         varchar(20)          null,
   category				int,
   constraint PK_TBL_HOTLINE primary key (id)
)
go

-------------------------------------------------------
-- 外键约束
alter table tbl_itsupport_arrangment
   add constraint FK_TBL_ITSU_REFERENCE_TBL_ITSU foreign key (ref_personID)
      references tbl_itsupport (id)
go

alter table tbl_link_data
   add constraint FK_TBL_LINK_REFERENCE_TBL_USER foreign key (ref_userID)
      references tbl_user (id)
go

alter table tbl_link_data
   add constraint FK_TBL_LINK_REFERENCE_TBL_CATE foreign key (ref_categoryID)
      references tbl_category (id)
go

-------------------------------------------------------
-- init data

DELETE tbl_category;
INSERT INTO tbl_category VALUES (N'Production');
INSERT INTO tbl_category VALUES (N'Management');


DELETE tbl_hotline;
INSERT INTO tbl_hotline VALUES (N'MES', '18926989171', 1);
INSERT INTO tbl_hotline VALUES (N'BAAN', '18666952949', 1);
INSERT INTO tbl_hotline VALUES (N'HuiMin', '18666952949', 2);

go

