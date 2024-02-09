/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Engine
	DROP CONSTRAINT DF_Engine_id
GO
CREATE TABLE dbo.Tmp_Engine
	(
	id uniqueidentifier NOT NULL ROWGUIDCOL,
	SerialNumber nvarchar(50) NULL,
	Hours numeric(18, 2) NOT NULL,
	Active bit NULL,
	unit uniqueidentifier NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Engine SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Engine ADD CONSTRAINT
	DF_Engine_id DEFAULT (newid()) FOR id
GO
ALTER TABLE dbo.Tmp_Engine ADD CONSTRAINT
	DF_Engine_Hours DEFAULT 0 FOR Hours
GO
IF EXISTS(SELECT * FROM dbo.Engine)
	 EXEC('INSERT INTO dbo.Tmp_Engine (id, SerialNumber, Hours, Active, unit)
		SELECT id, SerialNumber, CONVERT(numeric(18, 2), Hours), Active, unit FROM dbo.Engine WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Engine
GO
EXECUTE sp_rename N'dbo.Tmp_Engine', N'Engine', 'OBJECT' 
GO
ALTER TABLE dbo.Engine ADD CONSTRAINT
	PK_Engine PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.Engine', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Engine', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Engine', 'Object', 'CONTROL') as Contr_Per 


update Engine set Hours = (select Max(Activities.EnginePortTotalHours)      from Activities where Activities.EnginePort      = Engine.id group by Activities.EnginePort       ) where Hours =0 and (select Max(Activities.EnginePortTotalHours) from Activities where Activities.EnginePort = Engine.id group by Activities.EnginePort  ) > 0
go
update Engine set Hours = (select Max(Activities.EngineStarboardTotalHours) from Activities where Activities.EngineStarboard = Engine.id group by Activities.EngineStarboard  ) where Hours =0 and (select Max(Activities.EngineStarboardTotalHours) from Activities where Activities.EngineStarboard = Engine.id group by Activities.EngineStarboard  ) > 0

go