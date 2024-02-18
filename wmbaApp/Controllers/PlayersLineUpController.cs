using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using wmbaApp.Data;
using wmbaApp.Models;
using wmbaApp.ViewModels;

namespace wmbaApp.Controllers
{
    public class PlayersLineUpController : Controller
    {
        private readonly WmbaContext _context;

        public PlayersLineUpController(WmbaContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Index(string SearchString, int? ID,
             int? page, int? pageSizeID, string actionButton, string sortDirection = "asc", string sortField = "")
        {

            var data =  _context.PlayersLineUp.Include(x => x.Game).Include(x => x.Team).Include(x=>x.CompetitorTeam).Include(x=>x.CompetatorPlayers).Include(x => x.Players).AsNoTracking();


            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

            string[] sortOptions = new[] {  "Game Name", "Team Name", "Player Name" };
            ViewData["ID"] = new SelectList(_context.PlayersLineUp.ToList(), "Id", "GameLocation");
            //Add as many filters as needed
            if (ID.HasValue)
            {
                data = data.Where(t => t.Id == ID);
                numberFilters++;
            }

            if (!System.String.IsNullOrEmpty(SearchString))
            {
                data = data.Where(p => p.Game.GameLocation.ToUpper().Contains(SearchString.ToUpper())

                                       );

                numberFilters++;
            }
            //Give feedback about the state of the filters
            if (numberFilters != 0)
            {
                //Toggle the Open/Closed state of the collapse depending on if we are filtering
                ViewData["Filtering"] = " btn-danger";
                //Show how many filters have been applied
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
                //Keep the Bootstrap collapse open
                //@ViewData["ShowFilter"] = " show";
            }

            //Before we sort, see if we have called for a change of filtering or sorting
            if (!System.String.IsNullOrEmpty(actionButton)) //Form Submitted!
            {
                page = 1;//Reset page to start

                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }
            //Now we know which field and direction to sort by
            if (sortField == "Players LineUp Name")
            {

              
            }
            else if (sortField == "Game Name")
            {
                if (sortDirection == "asc")
                {
                    data = data
                      .OrderBy(p => p.Game.GameLocation);
                }
                else
                {
                    data = data
                       .OrderByDescending(p => p.Game.GameLocation);
                }
            }
            else if (sortField == "Team Name")
            {
                if (sortDirection == "asc")
                {
                    data = data
                       .OrderBy(p => p.Team.TmName);

                }
                else
                {
                    data = data
                       .OrderByDescending(p => p.Team.TmName);

                }
            }
           
           
           
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            //ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            //var pagedData = await PaginatedList<Game>.CreateAsync(data.AsNoTracking(), page ?? 1, pageSize);




            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int gameid)
        {
           
            var game = _context.Games
                .Include(t=>t.team.Players)
                .Include(t=>t.competitorTeam.Players)
               .FirstOrDefault(g => g.ID == gameid);

            //var data = _context.Games.FirstOrDefault(g => g.ID == gameid);
            if (game != null)
            {
                ViewBag.gameid = game.ID;
                ViewBag.hometeamId = game.teamId;
                ViewBag.awayteamId = game.competitorTeamId;
                ViewBag.hometeamname = game.team.TmName;
                ViewBag.awayteamname = game.competitorTeam.TmName;
                ViewBag.Hometeamplayers=  new SelectList(game.team.Players.ToList(), "ID", "PlyrFirstName");
                ViewBag.awayTeamplayers = new SelectList(game.competitorTeam.Players.ToList(), "ID", "PlyrFirstName");

            }

           // ViewBag.games = new SelectList(_context.Games.ToList(), "ID", "GameLocation");
            //ViewBag.Teams = new SelectList(_context.Teams.ToList(), "ID", "TmName");
           // ViewBag.CompetitorTeams = new SelectList(_context.Teams.ToList(), "ID", "TmName");
           // ViewBag.players = new SelectList(_context.Players.ToList(), "ID", "PlyrFirstName");
           // ViewBag.Compplayers = new SelectList(_context.Players.ToList(), "ID", "PlyrFirstName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,GameId,TeamId,PlayersIds,CompetitorTeamId,CompetitorTeamPlayerIds ")] PlayerLinUpVM playerLinUpVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var teamPlayers = await _context.Players
                   .Where(p => playerLinUpVM.PlayersIds.Contains(p.ID))
                   .ToListAsync();
                    var competitorTeamPlayer = await _context.Players
                     .Where(p => playerLinUpVM.CompetitorTeamPlayerIds.Contains(p.ID))
                     .ToListAsync();
                    var playerLineUp = new PlayersLineUp
                    {

                        GameId = playerLinUpVM.GameId,
                        TeamId = playerLinUpVM.TeamId,

                        CompetitorTeamId = playerLinUpVM.CompetitorTeamId,
                        // Other properties as needed
                        //Players = playerLinUpVM.PlayersIds
                        //.Select(playerId => new Player { ID = playerId })
                        //.ToList()


                        Players = teamPlayers,
                        CompetatorPlayers = competitorTeamPlayer

                    };

                    
                    
                    //playerLineUp.Players.AddRange(teamPlayers);
                    //playerLineUp.CompetatorPlayers.AddRange(competitorTeamPlayer);
                    await _context.PlayersLineUp.AddAsync(playerLineUp);
                   
                    await _context.SaveChangesAsync();
                  
                   
                    //await _context.PlayersLineUp.AddAsync(playerLinUpVM);
                    //await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

               

                // If the model is not valid, repopulate dropdowns and return to the view

                // Populate dropdown for Games
                

                return View(playerLinUpVM);

            }
            catch(Exception ex)
            {


                ViewBag.games = new SelectList(_context.Games.ToList(), "ID", "GameLocation", playerLinUpVM.GameId);
                ViewBag.Teams = new SelectList(_context.Teams.ToList(), "ID", "TmName", playerLinUpVM.TeamId);
                ViewBag.players = new SelectList(_context.Games.ToList(), "ID", "PlyrFirstName");
                ViewBag.CompetitorTeams = new SelectList(_context.Teams.ToList(), "ID", "TmName", playerLinUpVM.CompetitorTeamId);

                return View();
            }
            

            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Fetch the PlayersLineUp entity by id
            var playersLineUp = _context.PlayersLineUp
                .Include(pl => pl.Game)
                .Include(pl => pl.Team)
                .Include(pl => pl.Players)
               
                .FirstOrDefault(pl => pl.Id == id);

            if (playersLineUp == null)
            {
                return NotFound();
            }

            // Populate dropdowns for Games, Teams, and Players
            ViewBag.Games = new SelectList(_context.Games.ToList(), "ID", "GameLocation", playersLineUp.GameId);
            ViewBag.Teams = new SelectList(_context.Teams.ToList(), "ID", "TmName", playersLineUp.TeamId);
            ViewBag.Players = new MultiSelectList(_context.Players.ToList(), "ID", "PlyrFirstName", playersLineUp.Players.Select(p => p.ID));

            // Map the entity to the view model
            var viewModel = new PlayerLinUpVM
            {
                Id = playersLineUp.Id,
               
                GameId = playersLineUp.GameId,
                TeamId = playersLineUp.TeamId,
                PlayersIds = playersLineUp.Players.Select(p => p.ID).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GameId,TeamId,PlayersIds")] PlayerLinUpVM playerLinUpVM)
        {
            if (id != playerLinUpVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing PlayersLineUp entity
                    var playersLineUp = _context.PlayersLineUp
                        .Include(pl => pl.Players)
                        .FirstOrDefault(pl => pl.Id == id);

                    if (playersLineUp == null)
                    {
                        return NotFound();
                    }

                    // Update properties based on the view model
                   
                    playersLineUp.GameId = playerLinUpVM.GameId;
                    playersLineUp.TeamId = playerLinUpVM.TeamId;

                    // Update associated players
                    playersLineUp.Players = _context.Players.Where(p => playerLinUpVM.PlayersIds.Contains(p.ID)).ToList();

                    // Save changes
                    _context.Update(playersLineUp);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayersLineUpExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If the model is not valid, repopulate dropdowns and return to the view
            ViewBag.Games = new SelectList(_context.Games.ToList(), "ID", "GameLocation", playerLinUpVM.GameId);
            ViewBag.Teams = new SelectList(_context.Teams.ToList(), "ID", "TmName", playerLinUpVM.TeamId);
            ViewBag.Players = new MultiSelectList(_context.Players.ToList(), "ID", "PlyrFirstName", playerLinUpVM.PlayersIds);

            return View(playerLinUpVM);
        }


        public IActionResult Delete(int id)
        {
            var playersLineUp = _context.PlayersLineUp
                .Include(pl => pl.Game)
                .Include(pl => pl.Team)
                .Include(pl => pl.Players)
                .FirstOrDefault(pl => pl.Id == id);

            if (playersLineUp == null)
            {
                return NotFound();
            }

            return View(playersLineUp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playersLineUp = await _context.PlayersLineUp
                .Include(pl => pl.Players)
                .FirstOrDefaultAsync(pl => pl.Id == id);

            if (playersLineUp == null)
            {
                return NotFound();
            }

            // Remove associated players
            _context.Players.RemoveRange(playersLineUp.Players);

            // Remove the PlayersLineUp entity
            _context.PlayersLineUp.Remove(playersLineUp);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = _context.PlayersLineUp.
                Include(x => x.Game)
                .Include(x => x.Team)
                .Include(x => x.CompetitorTeam)
                .Include(x => x.CompetatorPlayers)
                .Include(x => x.Players)
                .FirstOrDefault(pl => pl.Id == id);


            //var playersLineUp = _context.PlayersLineUp
            //    .Include(pl => pl.Game)
            //    .Include(pl => pl.Team)
            //    .Include(pl=>pl.CompetitorTeam)
            //    .Include(pl => pl.Players)
            //     .Include(pl => pl.CompetatorPlayers)
            //    .FirstOrDefault(pl => pl.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }
        private bool PlayersLineUpExists(int id)
        {
            return _context.PlayersLineUp.Any(e => e.Id == id);
        }
        [HttpPost]
        public JsonResult GetPlayerByTeamId(int teamId)
        {
            var data = _context.Players.Where(x => x.TeamID == teamId).Select(x => new { x.ID, x.FullName }).ToList();
            return Json(data);
        }


        
        [HttpPost]
        public JsonResult GetPlayerByCmpatetorTeamId(int Compteamid)
        {
            var data = _context.Players.Where(x => x.TeamID == Compteamid).Select(x => new { x.ID, x.FullName }).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetTeamsByGameId(int gameid)
        {

            //var game =_context.Games.FirstOrDefault(x=>x.ID== gameid);
            //var data= 

            //     var game = _context.Teams.Where(x => x.GameTeams.Any(x => x.GameID == gameid));
            //     var teams = _context.GameTeams
            //.Where(gt => gt.GameID == gameid)
            //.Include(gt => gt.Team)
            //.Select(gt => gt.Team)
            //.Select(t => new { Id = t.ID, Name = t.TmName })
            //.ToList();


            // Ahmed :im stucked her this code subupe return teams related to game id but it doesnt and i dont know why
            var teams = _context.GameTeams
       .Where(gt => gt.GameID == gameid)
       .Select(gt => gt.Team)
       .Select(t => new { Id = t.ID, Name = t.TmName })
       .ToList();
            //var team = _context.Teams
            //.Where(t => t.GameTeams.Any(gt => gt.GameID == gameid))
            //.Select(t => new { Id = t.ID, Name = t.TmName })
            //ToList();
            //var competitorTeams = _context.Teams
            //.Where(t => t.GameTeams.Any(gt => gt.GameID == gameid))
            //.Select(t => new { Id = t.ID, Name = t.TmName })
            //.ToList();

            return Json(teams);
        }
        [HttpPost]
        public JsonResult GetComptTeamsByGameId(int gameid)
        {
            var teams = _context.GameTeams
       .Where(gt => gt.GameID == gameid)
       .Select(gt => gt.Team)
       .Select(t => new { Id = t.ID, Name = t.TmName })
       .ToList();
            //var team = _context.Teams
            //.Where(t => t.GameTeams.Any(gt => gt.GameID == gameid))
            //.Select(t => new { Id = t.ID, Name = t.TmName })
            //ToList();
            //var competitorTeams = _context.Teams
            //.Where(t => t.GameTeams.Any(gt => gt.GameID == gameid))
            //.Select(t => new { Id = t.ID, Name = t.TmName })
            //.ToList();
            return Json(teams);
        }
    }
}
