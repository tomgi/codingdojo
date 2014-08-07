--primesR a b = takeWhile (<= b) $ dropWhile (< a) $ sieve [2..]
import Debug.Trace
sieve [] = []
sieve (n:ns) = n:sieve [ m | m <- ns, m `mod` n /= 0 ]

main = print $ sieve [2..5]










