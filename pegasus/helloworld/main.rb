declare do
  ball <<-EOH
11111111
11111111
22222222
11111111
11111111
22222222
22222222
11111111
EOH

rocket <<-EOH
11111111
11111111
11111111
11111111
11111111
11111111
11111111
11111111
EOH

prawo 0
frame 0
end

scene do
  
  main_loop <<-EOH
    
    if frame == 0
      rocket.x=20
      rocket.y=20
      frame = 1
      ball.y=15
      ball.x=60
      # sprite "ball"      
    else

      # if prawo == 0
      #   ball.x-=2
      # end

      # if prawo == 1
      #   ball.x+=2
      # end

      # if ball.x <= 28 && ball.y+4 > rocket.y && ball.y+4 < rocket.y+16
      #   ball.x+=2
      #   prawo=1
      # end
          
      # if ball.x >= 255
      #   ball.x = 255
      #   prawo = 0
      # end

      # if ball.x <= 0
      #   ball.x = 0
      #   prawo = 1
      # end

      if is_pressed(:left)
        rocket.x-=1              
      end

      if is_pressed(:right)
        rocket.x+=1
      end
    end
    sprite "rocket"    
EOH
end