# B8CandidateTask
Candidate task

Configuration is needed in Bit8.StudentSystem.Web.Api - appsettings.json for configuring:
"ConnectionStrings": {
    "ApplicationDbContext": "Server=localhost;Database=bit8studentsystem;Uid=root;Pwd=1234;"
  }
  
 Database is auto seeded and created. Please, check the DB name not to exist in the DB, because it will REWRITTEN!!!!
 
 Data has 20 students with ids as follows:
Students in Semesters
1-10 1st	6-10 2nd	16-18 3rd
			11-15 2nd	19-20 none
	
Scores of students	
1-4 1st	semester		6-8 2nd	semester		18 3rd semester
5 none					11-12 2nd semester		16-17 none
6-7 1st semester		13-15 none				19-20 none
 
