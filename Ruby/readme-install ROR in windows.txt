1. - Install PostgreSQL 9.5 Windows
   - Turn Windows Feature on off -> Turn on Windows  Subsystem for linux
   - Install Ubuntu 16.04 from microsoft store
   - Run Ubuntu
   - Install Ruby 2.2.2 and ruby on Rails-> https://medium.com/@colinrubbert/installing-ruby-on-rails-in-windows-10-w-bash-postgresql-e48e55954fbf
2. Go to project -> open .env.example -> change DB_HOST value to localhost-> save as .env
3. Create Database "bluebird_ecv"
4. Using ubuntu go to directory -> cd /mnt/<directory>
5. Run "bundle Install"
6. Run "bundle exec rake db:migrate"
7. Run "bundle exec rake db:seed"
8. Run "rails s" 

