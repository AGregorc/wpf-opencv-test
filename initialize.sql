drop database if exists ec38;
create database ec38;
use ec38;


CREATE TABLE delovni_nalog (
   id              INT            NOT   NULL   AUTO_INCREMENT,
   st_kosov        INT,
   delovni_nalog   VARCHAR(30),
   PRIMARY   KEY   (id)
);



CREATE TABLE kosi (
   id              	INT            NOT   NULL   AUTO_INCREMENT,
   delovni_nalog_id INT,
   guid            	INT,
   cas_vnosa       	TIMESTAMP      NOT   NULL DEFAULT CURRENT_TIMESTAMP,
   PRIMARY   KEY   (id),
   FOREIGN KEY (delovni_nalog_id) 			REFERENCES delovni_nalog(id)
);

CREATE INDEX idx_guid ON kosi (guid);

