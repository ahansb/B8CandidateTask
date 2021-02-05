CREATE TABLE `bit8studentsystem`.`student` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `Surname` VARCHAR(45) NOT NULL,
  `DOB` DATE NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `id_UNIQUE` (`Id` ASC) VISIBLE);

CREATE TABLE `bit8studentsystem`.`semester` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `StartDate` DATE NOT NULL,
  `EndDate` DATE NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE);
  
CREATE TABLE `bit8studentsystem`.`discipline` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `DisciplineName` VARCHAR(45) NOT NULL,
  `ProfessorName` VARCHAR(45) NOT NULL,
  `SemesterId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `FK_Semester_idx` (`SemesterId` ASC) VISIBLE,
  CONSTRAINT `FK_Semester_Discipline`
    FOREIGN KEY (`SemesterId`)
    REFERENCES `bit8studentsystem`.`semester` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
	
CREATE TABLE `bit8studentsystem`.`score` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `StudentId` INT NOT NULL,
  `DisciplineId` INT NOT NULL,
  `Score` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `FK_Student_idx` (`StudentId` ASC) VISIBLE,
  INDEX `FK_Discipline_idx` (`DisciplineId` ASC) VISIBLE,
  CONSTRAINT `FK_Student_Score`
    FOREIGN KEY (`StudentId`)
    REFERENCES `bit8studentsystem`.`student` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_Discipline_Score`
    FOREIGN KEY (`DisciplineId`)
    REFERENCES `bit8studentsystem`.`discipline` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION);

CREATE TABLE `bit8studentsystem`.`studentsemester` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `StudentId` INT NOT NULL,
  `SemesterId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `FK_Semester_idx` (`SemesterId` ASC) VISIBLE,
  INDEX `FK_Student_idx` (`StudentId` ASC) VISIBLE,
  CONSTRAINT `FK_Student_StudentSemester`
    FOREIGN KEY (`StudentId`)
    REFERENCES `bit8studentsystem`.`student` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT `FK_Semester_StudentSemester`
    FOREIGN KEY (`SemesterId`)
    REFERENCES `bit8studentsystem`.`semester` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION);
