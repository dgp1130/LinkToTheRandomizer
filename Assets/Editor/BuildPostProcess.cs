#nullable enable

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

public sealed class BuildPostProcess
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // Only postprocess web builds.
        if (target != BuildTarget.WebGL) return;

        var index = Path.Combine(pathToBuiltProject, "index.html");
        var game = Path.Combine(pathToBuiltProject, "game.html");
        if (File.Exists(game)) File.Delete(game);
        File.Move(index, game);
        
        File.WriteAllText(index, indexPageContent);
    }

    private static readonly string indexPageContent = @$"
<!doctype html>
<html>
    <head>
        <title>Link to the Randomizer</title>
        <meta charset=""utf-8"">
    </head>
    <body>
        <main>
            <h2>Link to the Randomizer</h2>

            <aside>
                <ul>
                    <li><a href=""/game.html"">Check out the game!</a> (only supports keyboard, sorry!)</li>
                    <li><a href=""https://github.com/dgp1130/LinkToTheRandomizer/"">Check out the GitHub repo!</a></li>
                </ul>
            </aside>
        
            <article>
                <p>This is a simple tech demo for a ""randomizer"" game built from scratch. A ""randomizer"" in this context is
                a game which randomizes the core progression items and ask players to find the route through the game, picking
                up all the items they need to solve the puzzle and beat the game (often as quickly as possible). Many randomizers
                are mods and ROM hacks of existing games, this is great for breathing new life into an old and beloved title, but
                many design decisions for such games don't make sense in a randomizer context and aren't easily fixable.</p>

                <p>As one example, bombchus in Ocarina of Time allow players to reach any location which normally requires bombs,
                but they can waste bombchus at any time by just throwing them away, meaning the randomizer logic cannot assume the
                player uses bombchus effectively. Logic has to make a weird exception for this case, and they can't easily remove
                or alter bombchus as a mechanic without breaking many other aspects of the game. Had it been designed as a
                randomizer from the beginning, the devs likely never would have designed bombchus that way to begin with.</p>

                <p>The idea here is: ""What would it look like to build a game as a randomizer from scratch?"" This was the original
                motivation for the project, though that is a very broad design question that can't be easily answered in a week's
                worth of effort. Instead, this is just a basic tech demo, effectively remaking Link to the Past mechanics with a
                built in randomizer element. Maybe someday I'll do a blog post with my thoughts on what might work well in a
                ""designed from scratch"" randomizer.</p>

                <p>On the technical side, there are a few interesting aspects about the randomizer itself. Firstly, it has no
                knowledge of the game it is actually randomizing, with all logic and mechanics expressed via the YAML logic file.
                The logic file simply describes a graph with locks and keys. The locks describe the state of the game world and its
                mechanics, while the keys are randomly placed in a logically solvable fashion. It's quite simple and reusable across
                any game with a similar logic philosophy. It can already support features like allowing players to define specific
                glitches they can perform and will allow in logic, just by treating those as a key given to the player at game start.
                Other mechanics like a hint system, limiting keys to a particular area (dungeon), or a hand-made ""plandomizer"" would
                require some work, but would defintely be doable in a cross-game compatible fashion.</p>

                <p>Another cool aspect is that the randomization algorithm is upper-bounded and does not perform any kind of ""guess
                and check"" strategy. This means that it will always generate a valid seed on the first try and will never fail because
                ""I tried X times and failed to find a beatable seed"".</p>

                <p>Beyond that, I also just used this as an opportunity to learn <a href=""https://unity.com/"">Unity</a> and C#, as
                I'm not very familiar with either. Designing game systems in an object-oriented mindset like this is very different
                from what I'm used to as a web developer, so it's just a fun problem to think about.</p>

                <p>If this seems interesting, <a href=""/game.html"">try out the game!</a> (it takes like 10 seconds to beat) Or you
                can peek at the <a href=""https://github.com/dgp1130/LinkToTheRandomizer/"">source code</a> if you're interested in the
                implementation.</p>
            </article>
        </main>
    </body>
</html>
    ".Trim();
}
